using Il2Cpp;
using Il2CppPantheonPersist;
using MelonLoader;

namespace EnhancedDebuffTracking;

public static class EntityManager
{
    private static readonly string[] Blacklist = { "Banner of Arms", "Banner of Onslaught", "Challenger's Banner", "Rallying Banner", "Shieldman's Banner", "ghostly riddler" };
    // Global to hold the list of all debuffs for a monster, it accesses a List of debuffs via a unique monster id
    public static Dictionary<string, EntityData> gDebuffDictionary = new Dictionary<string, EntityData>();

    private static readonly List<EntityNpcGameObject> Monsters = new();
    private static readonly List<EntityNpcGameObject> FriendlyNPCs = new();

    public static EntityData GetEntityData(string targetNetworkId)
    {
        // We have no target selected
        if (targetNetworkId == null)
        {
            return null;
        }

        // EntityManager will remove entries from the Dictionary on death, not on despawn, so for now we just have to ignore all failures to find an enemy in the database
        // Not ideal as this will mask genuine problems but there is nothing we can do about it, it is how the hook for managing NPCs works
        if (gDebuffDictionary.ContainsKey(targetNetworkId))
        {
            return gDebuffDictionary[targetNetworkId];
        }
        else
        {
            return null;
        }
    }

    public static List<DebuffData> GetEntityDebuffList(string targetNetworkId)
    {
        // We have no target selected
        if (targetNetworkId == null)
        {
            return null;
        }

        // EntityManager will remove entries from the Dictionary on death, not on despawn, so for now we just have to ignore all failures to find an enemy in the database
        // Not ideal as this will mask genuine problems but there is nothing we can do about it, it is how the hook for managing NPCs works
        if (gDebuffDictionary.ContainsKey(targetNetworkId))
        {
            return gDebuffDictionary[targetNetworkId].debuffData;
        }
        else
        {
            return null;
        }
    }

    public static void UpdateAllDurationTimers()
    {
        for (int i = 0; i < gDebuffDictionary.Count; i++)
        {
            EntityData entityData = gDebuffDictionary.ElementAt(i).Value;
            List<DebuffData> debuffList = entityData.debuffData;
            // Process this list backwards, this allows us to delete from the end from inside the loop and not corrupt our own index
            for (int j = debuffList.Count()-1; j > -1; j--)
            {
                DebuffData debuff = debuffList[j];
                        
                // Sopmehow we have missed an event and this debuff has expired, remove it
                if (debuff.debuffDurationRemaining < 0)
                {
                    // This is not very nice, this is deleting an object inside the loop we created it, this MUST be the last thing we do in this function
                    debuffList.RemoveAt(j);
                }

                // Update the time remaining and the size of the progress bar
                debuff.debuffDurationRemaining = debuff.debuffDurationRemaining - 1;
                // Update uptime in seconds
                debuff.uptime++;

                // OnUpdate will certainly run before we can target a monster in range, handle the possible DIV0 error 
                if (entityData.startCombatTime == 0L)
                {
                    debuff.uptimePercent = 0L;
                }
                else
                {
                    long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    // If we add the first dbeuff to a monster just before OnUpdate runs we can get into this function before one whole second has passed
                    // There is no point in dealing with this edge case properly, set the combat start time back one second and continue processing,
                    // this will cause a minor inaccuracy in the uptime percent but its good enough for this mod and prevents a DIV0 error
//                    if (currentTime == entityData.startCombatTime)
                    //{
//                        // Reduce the start time by 1, one second doesnt matter in the grand scheme of things
//                        entityData.startCombatTime--;
//                    }

                    // Get the time in seconds the encounter has been running
                    float currentEncounterDurationS = (float)(currentTime - entityData.startCombatTime);
                    debuff.uptimePercent = (float)(debuff.uptime/ (float)(currentTime - entityData.startCombatTime)) * 100;
                    // Cap % at 100, this handles the case when the combat start time and current time are the same
                    if (debuff.uptimePercent > 100)
                    {
                        debuff.uptimePercent = 100;
                    }
                }
            }
        }
    }

    // This function checks is there is an entry in the dictionary for casterNetworkId and if not makes one, prevent exceptions if something unexpected happens
    public static void AddMonsterIfMissing(string targetNetworkId)
    {
        List<DebuffData> debuffData = GetEntityDebuffList(targetNetworkId);
        // We do not care about the return data
        if (debuffData == null)
        {
            EntityData newMonster = new EntityData();
            newMonster.startCombatTime = 0L;
            newMonster.isDead = false;
            newMonster.debuffData = new List<DebuffData>();
            gDebuffDictionary.Add(targetNetworkId, newMonster);
        }
    }

    public static void OnNpcAdded(EntityNpcGameObject entityNpcGameObject)
    {
        if (Monsters.Contains(entityNpcGameObject) || FriendlyNPCs.Contains(entityNpcGameObject))
        {
            return;
        }

        var npcName = entityNpcGameObject.Nameplate.nameText.text;

        if (entityNpcGameObject.Profession == NpcProfession.None)
        {
            Monsters.Add(entityNpcGameObject);

            if (entityNpcGameObject.Status.IsDead())
            {
                return;
            }

            // Weird behaviour in game, all NPCs have subname text set to Soandso's Minion, I guess as placeholder, but it
            // never displays this, so we'll rely on it I guess... sometimes minions are bugged and display as attackable
            // NPCs even if they're a player's summon. So we can't just rely on petmaster, as that's set to null in these cases.
            // I bet it's because the Summon enters the player's loadable area before the owner.
            if (entityNpcGameObject.PetMaster != null)
            {
                return;
            }

            var isOn = entityNpcGameObject.Nameplate.subNameText.isActiveAndEnabled;

            if (isOn)
            {
                return;
            }

            // Cheeky hack because the combat history and target is set to null when the npc pops in to the viewport, until
            // someone hits it the next time... so can't use those for determining if a mob is in combat.
            var pool = entityNpcGameObject.Pools.GetPool(PoolType.Health);
            var isFull = pool.Current == pool.Max;

            if (!isFull)
            {
                return;
            }

            if (Blacklist.Contains(npcName))
            {
                return;
            }

            Globals.MonstersInRange.Add(entityNpcGameObject.NetworkId.Value, entityNpcGameObject);
            Globals.MonstersInRangeLastPosition.Add(entityNpcGameObject.NetworkId.Value, entityNpcGameObject.transform.position);
            EntityData newMonster = new EntityData();
            newMonster.startCombatTime = 0L;
            newMonster.debuffData = new List<DebuffData>();
            gDebuffDictionary.Add(entityNpcGameObject.NetworkId.ToString(), newMonster);
        }
        else
        {
            FriendlyNPCs.Add(entityNpcGameObject);
        }
    }

    public static void OnNpcRemoved(EntityNpcGameObject entityNpcGameObject)
    {
        Monsters.Remove(entityNpcGameObject);
        Globals.MonstersInRange.Remove(entityNpcGameObject.NetworkId.Value);
        Globals.MonstersInRangeLastPosition.Remove(entityNpcGameObject.NetworkId.Value);
        //  Remove an entry from the dictionary based on the network id
        try
        {
            gDebuffDictionary.Remove(entityNpcGameObject.NetworkId.ToString());
        }
        catch (Exception e)
        {
            MelonLogger.Error($"OnNpcRemoved() Entry {entityNpcGameObject.NetworkId.ToString()} does not exist");
        }
        
    }
}