using A_Script;

[System.Serializable]
public class ItemData
{
    int _id;
    public int id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
            if (_id > 999999)
            {
                inventype = InventoryType.Equipment;
            }
            else
            {
                inventype = InventoryType.Skill;
            }
        }
    }

    float _factorpowmp;
    public float factorPowMP
    {
        get
        {
            return _factorpowmp;
        }
        set
        {
            _factorpowmp = (float)value / 10000f;
        }
    }

    float _factorpowap;
    public float factorPowAP
    {
        get
        {
            return _factorpowap;
        }
        set
        {
            _factorpowap = (float)value / 10000f;
        }
    }

    public InventoryType inventype;
    public string name;
    public string itemtype;
    public int type;
    public int grade;
    public string prefab;
    public int powMP;
    public int powMPDef;
    public int powAP;
    public int powAPDef;
    public int speed;
    public float range;

    public int enchantId;
    public int dismantleId;
    public int sell;
    public string description;
    public string atlasName;
    public string iconName;
    public bool E = false;
    public bool select = false;

    public virtual ItemData Copy()
    {
        ItemData id = new ItemData();
        id.id = this.id;
        id.name = name;
        id.itemtype = itemtype;
        id.type = type;
        id.grade = grade;
        id.prefab = prefab;
        id.powMP = powMP;
        id.powAP = powAP;
        id.speed = speed;
        id.range = range;

        id.enchantId = enchantId;
        id.dismantleId = dismantleId;
        id.sell = sell;
        id.description = description;
        id.atlasName = atlasName;
        id.iconName = iconName;
        return id;
    }

}