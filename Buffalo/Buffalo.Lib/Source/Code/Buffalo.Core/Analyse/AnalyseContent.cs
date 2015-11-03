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
<<<<<<< HEAD
=======
        public const string Check_Equal_ExpectedURL = "Check_Equal_ExpectedURL";

>>>>>>> origin/master
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
<<<<<<< HEAD
=======
                            refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Analyse StrContent Not Equal : " + sourceDataValue + " != " + objectDataValue, true);
                        }
                        else
                            refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Analyse StrContent Not Equal : " + sourceDataValue + " != " + objectDataValue, true);
                    }
                }
                else
                {
                    refTestCase.SingleInterrupt = true;
                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Analyse Exception : Can not find symbol - " + sourceDataName + " or " + dataObjectName, true);
                }
            }
        }

        public void Check_Equal_ExpectedURL(Case.BasicTestCase refTestCase, string currentURLDataName, string expectedURLDataName, bool isInterupt, Core.CodeProcessor.CodeLine activeSelectLine)
        {
            if (refTestCase != null)
            {
                if (refTestCase.ActiveDataBuffer.ContainsKey(currentURLDataName) && refTestCase.ActiveDataBuffer.ContainsKey(expectedURLDataName))
                {
                    string sourceDataValue = refTestCase.ActiveDataBuffer[currentURLDataName];
                    string objectDataValue = refTestCase.ActiveDataBuffer[currentURLDataName];

                    if (sourceDataValue != objectDataValue)
                    {
                        if (isInterupt)
                        {
                            refTestCase.SingleInterrupt = true;
>>>>>>> origin/master
                            refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Analyse Not Equal : " + sourceDataValue + " != " + objectDataValue, true);
                        }
                        else
                            refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Analyse Not Equal : " + sourceDataValue + " != " + objectDataValue, true);
                    }
                }
<<<<<<< HEAD
            }
        }
=======
                else
                {
                    refTestCase.SingleInterrupt = true;
                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Analyse Exception : Can not find symbol - " + currentURLDataName + " or " + expectedURLDataName, true);

                }
            }
        }

>>>>>>> origin/master
    }
}
