using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Core.Analyse
{

    public class AnalyseMap
    {
        public const string Check_Equal_StrContent = "Check_Equal_StrContent";
    }

    public class AnalyseContent
    {
        public void Check_Equal_StrContent(Case.BasicTestCase refTestCase, string sourceDataName, string dataObjectName, bool isInterupt, Core.CodeProcessor.CodeLine activeSelectLine)
        {
            if(refTestCase!=null)
            {
                if (refTestCase.ActiveDataBuffer.ContainsKey(sourceDataName) && refTestCase.ActiveDataBuffer.ContainsKey(dataObjectName))
                {
                    string sourceDataValue = refTestCase.ActiveDataBuffer[sourceDataName];
                    string objectDataValue = refTestCase.ActiveDataBuffer[dataObjectName];

                    if (sourceDataValue != objectDataValue)
                    {
                        if (isInterupt)
                        {
                            refTestCase.SingleInterrupt = true;
                            refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Analyse Not Equal : " + sourceDataValue + " != " + objectDataValue, true);
                        }
                        else
                            refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Analyse Not Equal : " + sourceDataValue + " != " + objectDataValue, true);
                    }
                }
            }
        }
    }
}
