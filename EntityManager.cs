using Il2Cpp;
using Il2CppPantheonPersist;
using Il2CppServiceStack;
using MelonLoader;
using Unity.Collections;
using Unity.Entities;
using static MelonLoader.MelonLogger;
using static UnityEngine.Rendering.HighDefinition.DebugDisplaySettings;


namespace EnhancedDebuffTracking;

public class ConsolidatedUptime()
{
    public string debuffName;
    public long totalEncounterUptime; // Time the debuff has been up as a % of total encounter time
    public float totalEncounterUptimePercent; // Time the debuff has been up as a % of total encounter time
}

public static class EntityManager
{
    

    private static readonly string[] Blacklist = { "Banner of Arms", "Banner of Onslaught", "Challenger's Banner", "Rallying Banner", "Shieldman's Banner", "ghostly riddler" };
    // Global to hold the list of all debuffs for a monster, it accesses a List of debuffs via a unique monster id
    private static Dictionary<string, EntityData> gMonsterDebuffDictionary = new Dictionary<string, EntityData>(); // MonsterId, EntityData>

    // TODO - These are very similar, can they be reduced to a single dictionary?
    private static Dictionary<string, List<ConsolidatedUptime>> consolidatedUptimeDictionary = new Dictionary<string, List<ConsolidatedUptime>>(); // MonsterId, List<debuffName, uptime>
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

    // Adds entry to calculate consolidated uptime for
    public static void AddConsolidatedUptime(string monsterNetworkId, DebuffData debuffData)
    {
        //MelonLogger.Warning($"AddConsolidatedUptime() monsterNetworkId = {monsterNetworkId}, debuffName = {debuffData.debuffName}");
        // if we do not have this debuff in our uptime dictionary, add it
        if (!consolidatedUptimeDictionary.ContainsKey(monsterNetworkId))
        {
            // Add a new entry with uptime of 0
            List<ConsolidatedUptime> newConsolidatedUptimeList = new List<ConsolidatedUptime>();
            ConsolidatedUptime newConsolidatedUptime = new ConsolidatedUptime();
            newConsolidatedUptime.debuffName = debuffData.debuffName;
            newConsolidatedUptime.totalEncounterUptime = 0;
            newConsolidatedUptime.totalEncounterUptimePercent = 0f;
            newConsolidatedUptimeList.Add(newConsolidatedUptime);
            consolidatedUptimeDictionary.Add(monsterNetworkId, newConsolidatedUptimeList);
        }
        else
        {
            List<ConsolidatedUptime> consolidatedUptimeList = consolidatedUptimeDictionary[monsterNetworkId];
            foreach(var temp in consolidatedUptimeList)
            {
                if (temp.debuffName == debuffData.debuffName)
                {
                    // This debuff already exists, dont add it twice
                    return;
                }
            }
            
            // Add this additional debuff
            ConsolidatedUptime consolidatedUptime = new ConsolidatedUptime();
            consolidatedUptime.debuffName = debuffData.debuffName;
            consolidatedUptime.totalEncounterUptime = 0;
            consolidatedUptime.totalEncounterUptimePercent = 0f;
            consolidatedUptimeList.Add(consolidatedUptime);
        }
    }

    // Gets the total consolidated uptime for a monster and debuff
    public static void IncrementConsolidatedUptime(string monsterNetworkId, string debuffName)
    {
        List<ConsolidatedUptime> consolidatedUptimeList = consolidatedUptimeDictionary[monsterNetworkId];
        if (!monsterNetworkId.IsEmpty() && consolidatedUptimeList != null && consolidatedUptimeList.Count > 0)
        {
            foreach (var uptimeItem in consolidatedUptimeList)
            {
                if (uptimeItem.debuffName == debuffName)
                {
                    uptimeItem.totalEncounterUptime++;
                }
            }
        }
    }

    // Gets the total consolidated uptime for a monster and debuff
    public static long GetConsolidatedUptime(string monsterNetworkId, string debuffName)
    {
        List<ConsolidatedUptime> ConsolidatedUptimeList = consolidatedUptimeDictionary[monsterNetworkId];
        foreach(var uptimeItem in ConsolidatedUptimeList)
        {
            if (uptimeItem.debuffName == debuffName)
            {
                return uptimeItem.totalEncounterUptime;
            }
        }

        return 0;
    }


    // Adds a debuff to the list of unique monsters debuffs, creates a new mosnter row if needed
    public static void addMonsterToUniqueDebuffs(string monsterNetworkId, string debuffName)
    {
        // Add a new monster to the list if this is the first time we are putting debuffs on it
        if (!uniqueDebuffsDictionary.ContainsKey(monsterNetworkId))
        {
            uniqueDebuffsDictionary.Add(monsterNetworkId, new List<string>());
        }

        // Add a new debuff to the list of debuffs if it does not already exist
        List<string> uniqueDebuffs = uniqueDebuffsDictionary[monsterNetworkId];
        if (!uniqueDebuffs.Contains(debuffName))
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
                // Update the time remaining and the size of the progress bar, stop at zero seconds
                debuff.debuffDurationRemaining = (debuff.debuffDurationRemaining == 0) ? 0 : debuff.debuffDurationRemaining - 1;
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
            // Incremement this
            gMonsterDebuffDictionary[monsterNetworkId].totalEncounterTime++;

            var uniqueMonsterDebuffList = uniqueDebuffsDictionary[monsterNetworkId];
            for (int i = 0; i < uniqueMonsterDebuffList.Count; i++)
            {
                // Find every monster debuff that matches this historic debuff name
                string currentHistoricDebuffName = uniqueMonsterDebuffList[i];

                // Update encounter uptime for this specific debuff in the list of all monster debuffs
                EntityData monster = gMonsterDebuffDictionary[monsterNetworkId];
                monster.monsterNetworkId = monsterNetworkId;

                // For every debuff on a monster
                foreach (DebuffData debuff in monster.debuffData)
                {
                    // If the debuff on the monster is the debuff we are looking for
                    if (debuff.debuffName == currentHistoricDebuffName)
                    {
                        // Match found, increase the encounter uptime only if the current duration remaining on the buff is > 0
                        if (debuff.debuffDurationRemaining > 0)
                        {
                            EntityManager.IncrementConsolidatedUptime(monster.monsterNetworkId, debuff.debuffName);
                            debuff.consolidatedEncounterUptime = EntityManager.GetConsolidatedUptime(monster.monsterNetworkId, debuff.debuffName);
                        }
                        
                        // OnUpdate will certainly run before we can target and engage a monster in range, prevent a possible DIV0
                        if (monster.encounterStartTime == 0L)
                        {
                            debuff.consolidatedEncounterUptimePercent = 0L;
                        }
                        else
                        {
                            // Get the time in seconds the encounter has been running
                            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            float currentEncounterDurationS = (float)(currentTime - monster.encounterStartTime);
                            debuff.consolidatedEncounterUptimePercent = (float)(debuff.consolidatedEncounterUptime / (float)(currentTime - monster.encounterStartTime)) * 100;
                            // Cap at 100 and 0, this handles the case when the combat start time and current time are the same
                            if (debuff.consolidatedEncounterUptimePercent > 100)
                            {
                                debuff.consolidatedEncounterUptimePercent = 100;
                            }
                            else if (debuff.consolidatedEncounterUptimePercent < 0)
                            {
                                debuff.consolidatedEncounterUptimePercent = 0;
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
            MelonLogger.Error("AddMonsterIfMissing() ADDING MISSING MONSTER");
            EntityData newMonster = new EntityData();
            newMonster.monsterNetworkId = targetNetworkId;
            //MelonLogger.Warning("AddMonsterIfMissing() is Dead = False");
            newMonster.isDead = false;
            newMonster.debuffData = new List<DebuffData>();
            gMonsterDebuffDictionary.Add(targetNetworkId, newMonster);
        }
    }

    public static void UpdateEnemyDeadStatus(EntityStatus.Logic entityStatusLogic)
    {
        if (entityStatusLogic == null)
            return;

        string networkId = entityStatusLogic.Entity.NetworkId.ToString();
        bool isDead = entityStatusLogic.Entity.Nameplate.isDead;

        if (gMonsterDebuffDictionary.ContainsKey(networkId.ToString()))
        {
            //MelonLogger.Warning($"MarkEnemyAsDead  networkId = {networkId}, isDead = {isDead}");
            gMonsterDebuffDictionary[networkId].isDead = isDead;
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

            // TODO - Get rid of the black list and the error showing buffs found on monsters loaded
            string[] debuffBlacklist = { "Mana Guzzle", "Taunt Immunity", "Feared", "Temporary Invulnerability" };
            // Pick up any traits if they exist
            // TODO - Do we want to pick up existing deuffs on monsters
            bool isFirst = true;
            foreach (ActiveBuff activeBuff in entityNpcGameObject.Buffs.myActiveBuffs)
            {
                string activeBuffName = activeBuff.BuffData.DisplayName.ToString();
                if (activeBuffName.Contains("Trait: "))
                {
                    string[] result = activeBuffName.Split("Trait: ");
                    if (result.Length > 1)
                    {
                        // We have a trait.  if this is the first trait, we dont want the leading comma
                        if (isFirst == true)
                        {
                            newMonster.traits = result[1];
                            isFirst = false;
                        }
                        else
                        {
                            newMonster.traits = newMonster.traits + ", " + result[1];
                        }
                    }
                }
                else
                {
                    // Do not process anything in the blacklist
                    if (!debuffBlacklist.Contains(activeBuffName.ToString()))
                    {
                        MelonLogger.Error($"OnNpcAdded() MONSTER WITH DEBUFF {activeBuffName} FOUND, DO WE WANT TO TRACK THIS ?");
                    }
                }
                // Increment the index
                
            }
            newMonster.isDead = entityNpcGameObject.Status.IsDead();
            
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
            //MelonLogger.Warning($"OnNpcRemoved() Entry {entityNpcGameObject.NetworkId.ToString()} Removed");
        }
        catch (Exception e)
        {
            MelonLogger.Warning($"OnNpcRemoved() Entry {entityNpcGameObject.NetworkId.ToString()} does not exist");
        }
        
    }
}