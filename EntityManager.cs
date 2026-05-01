using Il2Cpp;
using Il2CppPantheonPersist;
using Il2CppRootMotion;
using MelonLoader;

namespace EnhancedDebuffTracking;

public static class EntityManager
{
    private static readonly string[] Blacklist = { "Banner of Arms", "Banner of Onslaught", "Challenger's Banner", "Rallying Banner", "Shieldman's Banner", "ghostly riddler" };
    // Global to hold the list of all debuffs for a monster, it accesses a List of debuffs via a unique monster id
    public static Dictionary<string, List<DebuffData>> gDebuffDictionary = new Dictionary<string, List<DebuffData>>();

    private static readonly List<EntityNpcGameObject> Monsters = new();
    private static readonly List<EntityNpcGameObject> FriendlyNPCs = new();

    public static List<DebuffData> GetEnemyDebuffList(string targetNetworkId)
    {
        // If we have a valid key, return the list, else null
        if ( gDebuffDictionary.ContainsKey(targetNetworkId))
        {
            return gDebuffDictionary[targetNetworkId];
        }
        else
        {
            MelonLogger.Error($"GetEnemyDebuffList() - Could not find casterid {targetNetworkId} in dictionary");
            return null;
        }
    }

    // This function checks is there is an entry in the dictionary for casterNetworkId and if not makes one, prevent exceptions if something unexpected happens
    public static void AddMonsterIfMissing(string targetNetworkId)
    {
        List<DebuffData> temp = GetEnemyDebuffList(targetNetworkId);
        // We do not care about the return data
        if (temp == null)
        {
            List<DebuffData> newMonster = new List<DebuffData>();
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
            List<DebuffData> newMonster = new List<DebuffData>();
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
            MelonLogger.Warning($"OnNpcRemoved() Tried to remove entry {entityNpcGameObject.NetworkId.ToString()} but it did not exist");
        }
        
    }
}