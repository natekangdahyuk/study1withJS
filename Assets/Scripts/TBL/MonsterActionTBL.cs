using System.Collections.Generic;

public class MonsterActionTBL : AbstactReferenceContainer, ITBL
{
    IReferenceLoader _Loader = new FileReferenceLoader_Cvs();
    public void LoadData()
    {
        _Loader.InsertData_Event_Cvs = new InsertDataHandlerDelete_Cvs( InsertData );
        _Loader.Load( "table_mob_action" , "" );
    }

    private void InsertData( string[] node )
    {
        MonsterActionReferenceData new_data = new MonsterActionReferenceData();

        int dataCount = 0;
        new_data.ReferenceID = int.Parse( node[ dataCount++ ] );
        dataCount++;

        new_data.Name = int.Parse( node[ dataCount++ ] );
        dataCount++;

        new_data.ActionDesc = int.Parse( node[ dataCount++ ] );
        dataCount++;
        new_data.ActionIcon =  node[ dataCount++ ] ;
        new_data.actionType = ( ActionType )int.Parse( node[ dataCount++ ] );
        new_data.turn = int.Parse( node[ dataCount++ ] );
        new_data.AttValue = int.Parse( node[ dataCount++ ] );
        new_data.AttEffect = node[ dataCount++ ];
        new_data.DebuffTurn = int.Parse( node[ dataCount++ ] );
        new_data.DebuffNum = int.Parse( node[ dataCount++ ] );        
        FileReferenceLoader_Cvs.GetParseIntArrayByString(node[dataCount++], out new_data.DebuffValue);
        new_data.AttackEffect = node[ dataCount++ ];
        Add( new_data );
    }


    public static MonsterActionReferenceData GetData( int key )
    {
        MonsterActionTBL TBL = TBLManager.I.GetTable<MonsterActionTBL>( TABLELIST_TYPE.MonsterAction );
        return (MonsterActionReferenceData)TBL.Find( key );
    }
}