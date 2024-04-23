using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TFIKLabi
{
    //asgsdjhgdsjh.txt shjdfj.doc )))aekhwje.dtg////

    /// rfr.txt rere.tx rere.doc
    internal class Sertch
    {

        private string _fullText;
        private List<RegResult> FileNames = new List<RegResult>();
        StateMachineResult Results;

        //private List<(HtmlOpenTag, HtmlCloseTag)> _htmlDoublesTags = new List<(HtmlOpenTag, HtmlCloseTag)>(0);
        public Sertch(string Text)
        {
            _fullText = Text;
            //FileNames = Find(Text);
            Results = Find(Text);
        }



        public static StateMachineResult Find(string fulltext)
        {
            List<RegResult> FileNames = new List<RegResult>();
            StateMachineResult results = new StateMachineResult();
            string[] lines = fulltext.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int lineNumber = 1;

            foreach (string line1 in lines)
            {
                string line = line1+" ";
                StateMachine stateMachine = new StateMachine(line);

                int startIndex = 0;
                string filenameContent = "(Waiting)\n";

                results.AddResult(' ', State.Waiting);

                string filename = string.Empty;
                while (stateMachine.State != State.End)
                {
                    stateMachine.Next();

                    switch (stateMachine.State)
                    {
                        case State.Reading:
                        
                            filename += stateMachine.CurrentChar;
                            results.AddResult(stateMachine.CurrentChar, stateMachine.State);
                            break;

                        case State.StartReadEnd:
                            filename += stateMachine.CurrentChar;
                            results.AddResult(stateMachine.CurrentChar, stateMachine.State);
                            if (line[stateMachine.CurrentCharPos + 1] == 'p')
                            {
                                results.AddResult(stateMachine.NextChar, State.Pos_p);
                                filename += line[stateMachine.CurrentCharPos + 1];
                            } else if (line[stateMachine.CurrentCharPos + 1] == 't')
                            {
                                results.AddResult(stateMachine.NextChar, State.Pos_t1);
                                filename += line[stateMachine.CurrentCharPos + 1];
                            }
                            else if (line[stateMachine.CurrentCharPos + 1] == 'd')
                            {
                                results.AddResult(stateMachine.NextChar, State.Pos_d_docx);
                                filename += line[stateMachine.CurrentCharPos + 1];
                            }
                            else
                            {
                                results.AddResult(stateMachine.NextChar, State.Waiting.ToString() + " - False");
                            }
                            break;

                        case State.FinishReadEnd:
                            results.AddResult(stateMachine.NextChar, State.FinishReadEnd.ToString() + "-True");
                            //stateMachine.State = State.Waiting;

                            //filenameContent += " - (Waiting)\n";
                            RegResult reg = new RegResult(filename,  filenameContent,  startIndex, lineNumber);
                            FileNames.Add(reg);
                            startIndex = stateMachine.CurrentCharPos;
                            //filenameContent = string.Empty;
                            break;

                        case State.Waiting:
                            //filenameContent += " - " + stateMachine.CurrentChar;
                            //filenameContent += " - (Waiting)\n";
                            results.AddResult(stateMachine.CurrentChar, State.Waiting);
                            filename = string.Empty; 
                            break;

                        case State.Pos_p:
                            if (line[stateMachine.CurrentCharPos + 1] == 'd')
                            {
                                results.AddResult(stateMachine.NextChar, State.Pos_d_pdf);
                                //filenameContent += line[stateMachine.CurrentCharPos + 1];
                                filename += line[stateMachine.CurrentCharPos + 1];

                            }
                            else
                            {
                                results.AddResult(stateMachine.NextChar, State.Waiting.ToString() + " - False");
                            }
                            break;

                        case State.Pos_d_pdf:
                            if (line[stateMachine.CurrentCharPos + 1] == 'f')
                            {
                                results.AddResult(stateMachine.NextChar, State.FinishReadEnd);
                                filename += line[stateMachine.CurrentCharPos + 1];
                            }
                            else
                            {
                                results.AddResult(stateMachine.NextChar, State.Waiting.ToString() + " - False");
                            }
                            break;

                        case State.Pos_t1:
                            if (line[stateMachine.CurrentCharPos + 1] == 'x')
                            {
                                results.AddResult(stateMachine.NextChar, State.Pos_x_txt);
                                filename += line[stateMachine.CurrentCharPos + 1];
                            }
                            else
                            {
                                results.AddResult(stateMachine.NextChar, State.Waiting.ToString() + " - False");
                            }
                            break;
                            
                        case State.Pos_x_txt:
                            if (line[stateMachine.CurrentCharPos + 1] == 't')
                            {
                                results.AddResult(stateMachine.NextChar, State.FinishReadEnd);
                                //filenameContent += " - " + line[stateMachine.CurrentCharPos + 1];
                                filename += line[stateMachine.CurrentCharPos + 1];
                            }
                            else
                            {
                                results.AddResult(stateMachine.NextChar, State.Waiting.ToString() + " - False");
                            }
                            break;

                        case State.Pos_d_docx:
                            if (line[stateMachine.CurrentCharPos + 1] == 'o')
                            {
                                results.AddResult(stateMachine.NextChar, State.Pos_o);

//                                filenameContent += " - " + line[stateMachine.CurrentCharPos + 1];
                                filename += line[stateMachine.CurrentCharPos + 1];
                            }
                            else
                            {
                                results.AddResult(stateMachine.NextChar, State.Waiting.ToString() + " - False");
                            }
                            break;

                        case State.Pos_o:
                            if (line[stateMachine.CurrentCharPos + 1] == 'c')
                            {
                                results.AddResult(stateMachine.NextChar, State.Pos_cx);
                                //filenameContent += " - " + line[stateMachine.CurrentCharPos + 1];
                                filename += line[stateMachine.CurrentCharPos + 1];
                            }
                            else
                            {
                                results.AddResult(stateMachine.NextChar, State.Waiting.ToString() + " - False");
                            }
                            break;

                        case State.Pos_cx:
                            if (line[stateMachine.CurrentCharPos + 1] == 'x')
                            {
                                results.AddResult(stateMachine.NextChar, State.FinishReadEnd);
                                //filenameContent += " - " + line[stateMachine.CurrentCharPos + 1];
                                filename += line[stateMachine.CurrentCharPos + 1];

                            }else { results.AddResult(stateMachine.NextChar, State.Waiting); }
                            break;
                    }

                }
                lineNumber++;

            }
            return results;

        }


        public string GetResult()
        {
            //string result = "";

            //for (int i = 0; i < FileNames.Count; i++)
            //{   
            //    RegResult reg = FileNames[i];

            //        string report = $"Переход: {reg.resultcontent} Название файла: {reg.result} Позиция ({reg.start},{reg.start + reg.result.Length} Строка {reg.stringNum}) ";

            //        report += "\n";
            //        result += report;

            //}

            //return result;

            string result = "";

            for (int i = 0; i < Results.getCount(); i++)
            {

                string report = $"Символ: {Results.chars[i]} Состояние: {Results.states[i]}";

                report += "\n";
                result += report;

            }

            return result;

        }




    }
}
