using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

public delegate void InsertDataHandlerDelete(XmlNode node);
public delegate void InsertDataHandlerDelete_Cvs(string[] str);


public interface IReferenceLoader
{

    bool Load(string patch, string table_name);

    InsertDataHandlerDelete InsertData_Event
    {
        set;
    }

    InsertDataHandlerDelete_Cvs InsertData_Event_Cvs
    {
        set;
    }

}
