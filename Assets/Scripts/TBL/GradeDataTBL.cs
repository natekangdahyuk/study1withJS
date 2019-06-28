using System.Collections.Generic;

public class GradeDataTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs(InsertData);
        _Loader.Load("table_grade_data", "");
    }

    private void InsertData(string[] node)
    {
        GradeDataReferenceData new_data = new GradeDataReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse(node[dataCount++]);
        new_data.materialCount = int.Parse(node[dataCount++]);
        new_data.goldCost = int.Parse(node[dataCount++]);
        new_data.maxlv = int.Parse(node[dataCount++]);
        new_data.material_Exp = int.Parse(node[dataCount++]);
        new_data.stone = int.Parse(node[dataCount++]);


        Add(new_data);
    }


    public static GradeDataReferenceData GetData(int key)
    {
        GradeDataTBL TBL = TBLManager.I.GetTable<GradeDataTBL>(TABLELIST_TYPE.GradeData);
        return (GradeDataReferenceData)TBL.Find(key);
    }
}