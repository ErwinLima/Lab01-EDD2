﻿namespace Lab01
{
    public class AVLTree<T>
    {
        public Nodo<T>? Root;
        public AVLTree()
        {
            Root = null;
        }

        public bool IsEmpty() //Función para verificar si el árbol esta vacío
        {
            return Root == null;
        }

        public int GetBalance(Nodo<T> n1) //Función para obtener el factor de balance de un nodo
        {
            if(n1.Left == null && n1.Right == null)
            {
                return 0;
            }

            else if(n1.Left == null)
            {
                return -n1.Right!.Height;
            }

            else if(n1.Right == null)
            {
                return n1.Left!.Height;
            }

            else
            {
                return n1.Left!.Height - n1.Right!.Height;
            }
        }

        private void SetHeight(Nodo<T> nodo) //Procedimiento para asignar la altura de un nodo
        {
            //Se toma el nodo hijo con mayor altura
            if (nodo.Left == null || nodo.Right == null)
            {
                if (nodo.Left == null && nodo.Right == null)
                {
                    nodo.Height = 1;
                }

                else if (nodo.Left == null)
                {
                    nodo.Height = 1 + nodo.Right!.Height;
                }

                else
                {
                    nodo.Height = 1 + nodo.Left!.Height;
                }
            }

            else if (nodo.Left!.Height > nodo.Right!.Height) //Nodo 'One' mayor?
            {
                nodo.Height = 1 + nodo.Left!.Height; //Si
            }
            else
            {
                nodo.Height = 1 + nodo.Right!.Height; //No
            }
        }

        public void Add(T item, Delegate condition1, Delegate condition2) //Procedimiento para añadir elementos al árbol
        {
            Root = AddInAVL(Root!, item, condition1, condition2);
        }

        private Nodo<T> AddInAVL(Nodo<T>? nodo, T item, Delegate condition1, Delegate condition2) 
        {
            if(nodo == null)
            {
                nodo = new Nodo<T>(item);
            }
            else
            {
                if((int)condition1.DynamicInvoke(item, nodo!.Value) < 0) //El valor a agregar es menor al valor del nodo (condicion 1)
                {
                    nodo.Left = AddInAVL(nodo.Left!, item, condition1, condition2);
                }

                else if((int)condition1.DynamicInvoke(item, nodo!.Value!) > 0)
                {
                    nodo.Right = AddInAVL(nodo.Right!, item, condition1, condition2);
                }
                else //Son iguales
                {
                    if ((int)condition2.DynamicInvoke(item, nodo!.Value) < 0) //El valor a agregar es menor al valor del nodo
                    {
                        nodo.Left = AddInAVL(nodo.Left!, item, condition1, condition2);
                    }
                    else if ((int)condition2.DynamicInvoke(item, nodo!.Value!) > 0)
                    {
                        nodo.Right = AddInAVL(nodo.Right!, item, condition1, condition2);
                    }
                }
            }

            nodo = ReBalance(nodo);
            return nodo;
        }

        public void Delete(T item, Delegate CompareObj)
        {
            Root = DeleteInAVL(Root!, item, CompareObj);
        }

        public Nodo<T> DeleteInAVL(Nodo<T>? nodo, T item, Delegate CompareObj)
        {
            if (nodo == null)
            {
                return nodo;
            }

            else if((int)CompareObj.DynamicInvoke(item, nodo.Value) < 0)
            {
                nodo.Left = DeleteInAVL(nodo.Left!, item, CompareObj);
            }

            else if ((int)CompareObj.DynamicInvoke(item, nodo.Value) > 0)
            {
                nodo.Right = DeleteInAVL(nodo.Right!, item, CompareObj);
            }

            else
            {
                if(nodo.Left == null && nodo.Right == null) //Es una hoja
                {
                    nodo = null;
                    return nodo;
                }

                else if(nodo.Left == null && nodo.Right != null)
                {
                    Nodo<T> temp = nodo.Right;
                    nodo.Right = null;
                    nodo = temp;
                }

                else if (nodo.Left != null && nodo.Right == null)
                {
                    Nodo<T> temp = nodo.Left;
                    nodo.Left = null;
                    nodo = temp;
                }

                else
                {
                    Nodo<T>? temp = RightestfromLeft(nodo.Left!);
                    nodo.Value = temp!.Value;
                    nodo.Left = DeleteInAVL(nodo.Left!, temp.Value, CompareObj);
                }
            }

            nodo = ReBalance(nodo);
            return nodo;
        }

        private Nodo<T> ReBalance(Nodo<T> nodo)
        {
            SetHeight(nodo); //Se actualiza la altura del nodo
            int FE = GetBalance(nodo); //Se obtiene el factor de equilibrio del nodo
            if(FE < -1)
            {
                if(GetBalance(nodo.Right!) < 0)
                {
                    nodo = LeftRotation(nodo);//Rotación izquierda
                }
                else
                {
                    nodo = DoubleLeftRotation(nodo);//Rotación doble a la izquierda
                }
            }

            else if (FE > 1)
            {
                if (GetBalance(nodo.Left!) > 0)
                {
                    nodo = RightRotation(nodo);//Rotación derecha
                }
                else
                {
                    nodo = DoubleRightRotation(nodo);//Rotación doble a la derecha
                }
            }

            return nodo;
        }

        private Nodo<T> RightRotation(Nodo<T> nodo) //Rotación simple a la derecha
        {
            Nodo<T> temp = nodo.Left!;
            nodo.Left = temp.Right!;
            temp.Right = nodo;
            SetHeight(nodo);
            SetHeight(temp);
            return temp;
        }

        private Nodo<T> LeftRotation(Nodo<T> nodo) //Rotación simple a la izquierda
        {
            Nodo<T> temp = nodo.Right!;
            nodo.Right = temp.Left!;
            temp.Left = nodo;
            SetHeight(nodo);
            SetHeight(temp);
            return temp;
        }

        private Nodo<T> DoubleRightRotation(Nodo<T> nodo) //Rotación doble a la derecha
        {
            Nodo<T> temp = nodo.Left!;
            temp = LeftRotation(temp);
            nodo.Left = temp;
            nodo = RightRotation(nodo);
            return nodo;
        }

        private Nodo<T> DoubleLeftRotation(Nodo<T> nodo) //Rotación doble a la izquierda
        {
            Nodo<T> temp = nodo.Right!;
            temp = RightRotation(temp);
            nodo.Right = temp;
            nodo = LeftRotation(nodo);
            return nodo;
        }

        private Nodo<T>? RightestfromLeft(Nodo<T>? nodo) //Función que devuelve el nodo más grande del subárbol izquierdo
        {
            Nodo<T> nodoTemp = nodo;
            while (nodoTemp!.Right != null)
            {
                nodoTemp = nodoTemp.Right;
            }
            return nodoTemp;
        }
    }
}

