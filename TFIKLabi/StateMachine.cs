using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFIKLabi
{
    public enum State
    {
        Waiting,
        Reading,
        End,
        StartReadEnd,
        FinishReadEnd,
        Pos_p,
        Pos_d_pdf,
        Pos_f,
        Pos_t1, Pos_t2,
        Pos_x_txt,
        Pos_d_docx,
        Pos_o,
        Pos_cx,
        Pos_x_docx,
    }

    internal class StateMachine
    {
        private string Line;
        public int CurrentCharPos;
        public char CurrentChar => Line[CurrentCharPos];
        public char NextChar => Line[CurrentCharPos+1];

        public State State;
        public StateMachine(string line) 
        {
            Line = line;   
            CurrentCharPos = -1;
            State = State.Waiting;
        }

        public void Next()
        {
            CurrentCharPos++;

            if (CurrentCharPos == Line.Length)
            {
                State = State.End;
                return;
            }

            if ((State == State.Pos_d_pdf) && "/ & # ,   [ ] { } ( )".Contains(NextChar.ToString()))
            {
                State = State.FinishReadEnd;
                return;
            }
            if ((State == State.Pos_x_txt) && "/ & # ,   [ ] { } ( )".Contains(NextChar.ToString()))
            {
                State = State.FinishReadEnd;
                return;
            }
            if ((State == State.Pos_cx) && "/ & # ,   [ ] { } ( )".Contains(NextChar.ToString()))
            {
                State = State.FinishReadEnd;
                return;
            }
            if ((State == State.Pos_o) && "/ & # ,   [ ] { } ( )".Contains(NextChar.ToString()))
            {
                State = State.FinishReadEnd;
                return;
            }

            switch (CurrentChar)
            {
                case '.': State = State.StartReadEnd; break;
                case '/': State = State.Waiting; break;
                case ' ': State = State.Waiting; break;
                case '$': State = State.Waiting; break;
                case '&': State = State.Waiting; break;
                case '#': State = State.Waiting; break;
                case ',': State = State.Waiting; break;
                case '(': State = State.Waiting; break;
                case ')': State = State.Waiting; break;
                case '{': State = State.Waiting; break;
                case '}': State = State.Waiting; break;
                case '[': State = State.Waiting; break;
                case ']': State = State.Waiting; break;
                case 'p':
                    if (State == State.StartReadEnd) { State = State.Pos_p; break; } 
                    else { State = State.Reading; break; }


                case 'd':
                    if (State == State.Pos_p) { State = State.Pos_d_pdf; break; }
                    else if (State == State.StartReadEnd) { State = State.Pos_d_docx; break; } 
                    else { State = State.Reading; break; }


                case 'f':
                    if (State == State.Pos_d_pdf) { State = State.FinishReadEnd; break; } 
                    //if (State == State.Pos_d_pdf) { State = State.Pos_f; break; } 
                    else { State = State.Reading; break; }


                case 't':
                    if (State == State.StartReadEnd ) { State = State.Pos_t1; break; }
                    // else if (State == State.Pos_x_txt) { State = State.Pos_t2; break; } 
                    else if (State == State.Pos_x_txt) { State = State.FinishReadEnd; break; } 
                    else { State = State.Reading; break; }
                    

                case 'x':
                    if (State == State.Pos_t1) { State = State.Pos_x_txt; break; } 
                    else if (State == State.Pos_cx) { State = State.FinishReadEnd; break; } 
                    //else if (State == State.Pos_cx) { State = State.Pos_x_docx; break; } 
                    else { State = State.Reading; break; }
                

                case 'o':
                    if (State == State.Pos_d_docx) { State = State.Pos_o; break;}
                    else { State = State.Reading; break; }


                case 'c':
                    if (State == State.Pos_o) { State = State.FinishReadEnd; break; }
                    //if (State == State.Pos_o) { State = State.Pos_cx; break; }
                    else { State = State.Reading; break; }


                default: State = State.Reading; break;
            }
            


            

        }
    }


}
