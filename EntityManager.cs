using Il2Cpp;
using Il2CppPantheonPersist;
using Il2CppServiceStack;
using Il2CppViNL;
using MelonLoader;
using Unity.Collections;
using static UnityEngine.Rendering.HighDefinition.DebugDisplaySettings;

namespace EnhancedDebuffTracking;

public class ConsolidatedUptime()
{
    public string debuffName;
    public int uptime;
}

public static class EntityManager
{
    

    private static readonly string[] Blacklist = { "Banner of Arms", "Banner of Onslaught", "Challenger's Banner", "Rallying Banner", "Shieldman's Banner", "ghostly riddler" };
    // Global to hold the list of all debuffs for a monster, it accesses a List of debuffs via a unique monster id
    private static Dictionary<string, EntityData> gMonsterDebuffDictionary = new Dictionary<string, EntityData>(); // MonsterId, EntityData>
    private static Dictionary<string, ConsolidatedUptime> uptimeDictionary = new Dictionary<string, ConsolidatedUptime>(); // MonsterId, List<debuffName, uptime>
    private static Dictionary<string, List<string>> uniqueDebuffsDictionary = new Dictionary<string, List<string>>(); // MonsterId, List<debuffName>

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
        if (gMonsterDebuffDictionary.ContainsKey(targetNetworkId))
        {
            return gMonsterDebuffDictionary[targetNetworkId];
        }
        else
        {
            return null;
        }
    }

    // Adds bebuffs to a calculate uptime for an encounter
    public static void AddDebuffToUptime(string monsterNetworkId, DebuffData debuffData)
    {
        // if we do not have this debuff in our uptime dictionary, add it
        if (!uptimeDictionary.ContainsKey(monsterNetworkId))
        {
            // Add a new entry with uptime of 0
            ConsolidatedUptime consolidatedUptime = new ConsolidatedUptime();
            consolidatedUptime.debuffName = debuffData.debuffName;
            consolidatedUptime.uptime = 0;
            uptimeDictionary.Add(monsterNetworkId, consolidatedUptime);
        }
    }

    // Adds a dbeuff to the list of unique monsters debuffs, creates a new mosnter row if needed
    public static void addMonsterToUniqueDebuffs(string monsterNetworkId, string debuffName)
    {
        // Add a new monster to the list if this is the first time we are putting debuffs on it
        if (!uniqueDebuffsDictionary.ContainsKey(monsterNetworkId))
        {
            uniqueDebuffsDictionary.Add(monsterNetworkId, new List<string>());
        }

        // Add a new debuff to the list of debuffs if it does not already exist
        List<string> uniqueDebuffs = uniqueDebuffsDictionary[monsterNetworkId];
        if (uniqueDebuffs.Contains(debuffName))
        {
            return;
        }
        else
        {
            uniqueDebuffs.Add(debuffName);
        }
    }

    // This removes a monster from the list of monsters with unique debuffs
    public static void removeMonsterFromUniqueBuffs(string monsterNetworkId)
    {
        if (uniqueDebuffsDictionary.ContainsKey(monsterNetworkId)) {
            uniqueDebuffsDictionary.Remove(monsterNetworkId);
        }
    }

    // This function updates the duration remaining for all the progress bars
    public static void UpdateDurationRemaining()
    {
        for (int i = 0; i < gMonsterDebuffDictionary.Count; i++)
        {
            EntityData entityData = gMonsterDebuffDictionary.ElementAt(i).Value;
            List<DebuffData> debuffData = entityData.debuffData;

            // For all debuffs for this monster
            for (int j = 0; j < debuffData.Count; j++)
            {
                DebuffData debuff = debuffData.ElementAt(j);
                // Update the time remaining and the size of the progress bar
                debuff.debuffDurationRemaining = debuff.debuffDurationRemaining - 1;                
            } // End of FOR all debuffs for a monster
        } // End of FOR all monsters
    }


    // This function updates the uptime for all active debuffs for all monsters
    public static void UpdateEncounterUpTime()
    {
        // We need to handle the folllowing scenarios
        // 1) Update the uptime value of an active debuff 
        // 2) Update the uptime value of a debuff that has dropped off the list of active debuffs but might be reapplied later on

        // For any debuff we have ever had for this monster
        var allMonsterNetworkIds = uniqueDebuffsDictionary.Keys;
        foreach (var monsterNetworkId in allMonsterNetworkIds)
        {
            var uniqueMonsterDebuffList = uniqueDebuffsDictionary[monsterNetworkId];
            for (int i = 0; i < uniqueMonsterDebuffList.Count; i++)
            {
                // Find every monster debuff that matches this historic debuff name
                string currentHistoricDebuffName = uniqueMonsterDebuffList[i];

                // Update encounter uptime for this specific debuff in the list of all monster debuffs
                EntityData monster = gMonsterDebuffDictionary[monsterNetworkId];
                foreach(DebuffData debuff in monster.debuffData)
                {
                    if (debuff.debuffName == currentHistoricDebuffName)
                    {
                        // Match found, increase the encounter uptime only if the current duration remaining on the buff is > 0
                        if (debuff.debuffDuration > 0)
                        {
                            debuff.totalEncounterUptime++;
                        }
                        
                        MelonLogger.Warning($"UpdateEncounterUpTime() monster.totalEncounterUptime = {debuff.totalEncounterUptime}");
                        // OnUpdate will certainly run before we can target and engage a monster in range, prevent a possible DIV0
                        if (monster.encounterStartTime == 0L)
                        {
                            debuff.totalEncounterUptimePercent = 0L;
                            MelonLogger.Warning($"UpdateEncounterUpTime() encounterStartTime = 0");
                        }
                        else
                        {
                            // Get the time in seconds the encounter has been running
                            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            MelonLogger.Warning($"UpdateEncounterUpTime() currentTime = {currentTime}");
                            MelonLogger.Warning($"UpdateEncounterUpTime() monster.encounterStartTime = {monster.encounterStartTime}");
                            float currentEncounterDurationS = (float)(currentTime - monster.encounterStartTime);
                            MelonLogger.Warning($"UpdateEncounterUpTime() currentEncounterDurationS = {currentEncounterDurationS}");
                            debuff.totalEncounterUptimePercent = (float)(debuff.totalEncounterUptime / (float)(currentTime - monster.encounterStartTime)) * 100;
                            MelonLogger.Warning($"UpdateEncounterUpTime() monster.totalEncounterUptimePercent = {debuff.totalEncounterUptimePercent}");
                            // Cap % at 100, this handles the case when the combat start time and current time are the same
                            if (debuff.totalEncounterUptimePercent > 100)
                            {
                                debuff.totalEncounterUptimePercent = 100;
                            }
                            else if (debuff.totalEncounterUptimePercent < 0)
                            {
                                debuff.totalEncounterUptimePercent = 0;
                            }
                        }
                    }
                }
            }
        }
    }


    // This function updates the uptime for all active debuffs for all monsters
    public static void UpdateLocalUpTime()
    {
        for (int i = 0; i < gMonsterDebuffDictionary.Count; i++)
        {
            EntityData entityData = gMonsterDebuffDictionary.ElementAt(i).Value;
            List<DebuffData> debuffData = entityData.debuffData;

            // For all debuffs for this monster
            for (int j = 0; j < debuffData.Count; j++)
            {
                DebuffData debuff = debuffData.ElementAt(j);
                // Update uptime in seconds
                debuff.localUptime++;

                // OnUpdate will certainly run before we can target and engage a monster in range, prevent a possible DIV0
                if (entityData.encounterStartTime == 0L)
                {
                    debuff.localUptimePercent = 0L;
                }
                else
                {
                    // Get the time in seconds the encounter has been running
                    long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    float currentEncounterDurationS = (float)(currentTime - entityData.encounterStartTime);
                    debuff.localUptimePercent = (float)(debuff.localUptime / (float)(currentTime - entityData.encounterStartTime)) * 100;
                    // Cap % at 100, this handles the case when the combat start time and current time are the same
                    if (debuff.localUptimePercent > 100)
                    {
                        debuff.localUptimePercent = 100;
                    }
                    else if (debuff.localUptimePercent < 0)
                    {
                        debuff.localUptimePercent = 0;
                    }
                }
            } // End of FOR all debuffs for a monster
        } // End of FOR all monsters
    }

    // This function parses the list of all debuffs on all monsters and if something has hit -1 seconds and we havent received an event to remove it, we remove it manually
    public static void RemoveZombiedDebuffs()
    {
        for (int i = 0; i < gMonsterDebuffDictionary.Count; i++)
        {
            EntityData entityData = gMonsterDebuffDictionary.ElementAt(i).Value;
            List<DebuffData> debuffList = entityData.debuffData;
            // Process this list backwards, this allows us to delete from the end from inside the loop and not corrupt our own index
            for (int j = debuffList.Count() - 1; j > -1; j--)
            {
                DebuffData debuff = debuffList[j];

                // Sopmehow we have missed an event and this debuff has expired, remove it
                if (debuff.debuffDurationRemaining < 0)
                {
                    // This is not very nice, this is deleting an object inside the loop we created it
                    MelonLogger.Error($"RemoveZombiedDebuffs() Found Zombie.  debuff.debuffName = {debuff.debuffName}, debuff.targetName = {debuff.targetName}, debuff.casterName = {debuff.casterName}");
                    debuffList.RemoveAt(j);
                }
            }
        }
    }

    // This function checks is there is an entry in the dictionary for casterNetworkId and if not makes one, prevent exceptions if something unexpected happens
    public static void AddMonsterIfMissing(string targetNetworkId)
    {
        EntityData entityData = EntityManager.GetEntityData(targetNetworkId);        
        // Make a new entity if one does not exist
        if (entityData == null)
        {
            EntityData newMonster = new EntityData();
            newMonster.isDead = false;
            newMonster.debuffData = new List<DebuffData>();
            gMonsterDebuffDictionary.Add(targetNetworkId, newMonster);
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
            newMonster.debuffData = new List<DebuffData>();
            if (gMonsterDebuffDictionary.ContainsKey(entityNpcGameObject.NetworkId.ToString()))
            {
                MelonLogger.Error($"OnNpcAdded() Entry {entityNpcGameObject.NetworkId.ToString()} already exists in the dictionary, this should never happen");
            }
            gMonsterDebuffDictionary.Add(entityNpcGameObject.NetworkId.ToString(), newMonster);
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
            removeMonsterFromUniqueBuffs(entityNpcGameObject.NetworkId.ToString());
            gMonsterDebuffDictionary.Remove(entityNpcGameObject.NetworkId.ToString());
            //wMelonLogger.Warning($"OnNpcRemoved() Entry {entityNpcGameObject.NetworkId.ToString()} Removed");
        }
        catch (Exception e)
        {
            MelonLogger.Warning($"OnNpcRemoved() Entry {entityNpcGameObject.NetworkId.ToString()} does not exist");
        }
        
    }
}