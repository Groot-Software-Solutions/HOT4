namespace Hot.Econet.Prepaid.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class methodResponse
{

    private methodResponseParams paramsField;

    /// <remarks/>
    public methodResponseParams @params
    {
        get
        {
            return this.paramsField;
        }
        set
        {
            this.paramsField = value;
        }
    }
}



/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class methodResponseParams
{

    private methodResponseParamsParam paramField;

    /// <remarks/>
    public methodResponseParamsParam param
    {
        get
        {
            return this.paramField;
        }
        set
        {
            this.paramField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class methodResponseParamsParam
{

    private methodResponseParamsParamValue valueField;

    /// <remarks/>
    public methodResponseParamsParamValue value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class methodResponseParamsParamValue
{

    private methodResponseParamsParamValueArray arrayField;

    /// <remarks/>
    public methodResponseParamsParamValueArray array
    {
        get
        {
            return this.arrayField;
        }
        set
        {
            this.arrayField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class methodResponseParamsParamValueArray
{

    private methodResponseParamsParamValueArrayValue[] dataField;

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("value", IsNullable = false)]
    public methodResponseParamsParamValueArrayValue[] data
    {
        get
        {
            return this.dataField;
        }
        set
        {
            this.dataField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class methodResponseParamsParamValueArrayValue
{

    private methodResponseParamsParamValueArrayValueMember[] structField;

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("member", IsNullable = false)]
    public methodResponseParamsParamValueArrayValueMember[] @struct
    {
        get
        {
            return this.structField;
        }
        set
        {
            this.structField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class methodResponseParamsParamValueArrayValueMember
{

    private string nameField;

    private methodResponseParamsParamValueArrayValueMemberValue valueField;

    /// <remarks/>
    public string name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    public methodResponseParamsParamValueArrayValueMemberValue value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class methodResponseParamsParamValueArrayValueMemberValue
{

    private ulong intField;

    /// <remarks/>
    public ulong @int
    {
        get
        {
            return this.intField;
        }
        set
        {
            this.intField = value;
        }
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



