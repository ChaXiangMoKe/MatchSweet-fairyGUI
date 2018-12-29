using System;
using System.Collections.Generic;

public interface IResetable
{
    void Reset();
}

public class Pool<T> : PoolBase where T :class ,IResetable ,new ()
{ 
    private Stack<T> m_objectSatck;
    private Action<T> m_resetAction;
    private  Action<T> m_onetimeInitAction;

    public Pool(int initialBufferSize,Action<T> ResetAction = null, Action<T> OnetimeInitAction = null)
    {
        m_objectSatck = new Stack<T>(initialBufferSize);
        m_resetAction = ResetAction;
        m_onetimeInitAction = OnetimeInitAction;
    }
    public T New()
    {
        if(m_objectSatck.Count > 0)
        {
            T t = m_objectSatck.Pop();
            t.Reset();

            if(m_resetAction != null)
            {
                m_resetAction(t);
            }
            return t;
        }
        else
        {
            T t = new T();
            if(m_onetimeInitAction != null)
            {
                m_onetimeInitAction(t);
            }
            return t;
        }
    }

    public void Store(T obj)
    {
        m_objectSatck.Push(obj);
    }    
}
