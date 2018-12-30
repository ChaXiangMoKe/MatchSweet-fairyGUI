using System;
using System.Collections.Generic;

public interface IResetable
{
    void Reset();
}

public class Pool
{
    private Stack<object> m_objectSatck;
    private Action<object> m_resetAction;
    private Action<object> m_onetimeInitAction;

    public Pool(int initialBufferSize, Action<object> ResetAction = null, Action<object> OnetimeInitAction = null)
    {
        m_objectSatck = new Stack<object>(initialBufferSize);
        m_resetAction = ResetAction;
        m_onetimeInitAction = OnetimeInitAction;
    }
    public T Create<T>() where T : class, IResetable, new()
    {
        if (m_objectSatck.Count > 0)
        {
            T t = m_objectSatck.Pop() as T;
            t.Reset();

            if (m_resetAction != null)
            {
                m_resetAction(t);
            }
            return t;
        }
        else
        {
            T t = new T();
            if (m_onetimeInitAction != null)
            {
                m_onetimeInitAction(t);
            }
            return t;
        }
    }

    public void Store(object obj)
    {
        m_objectSatck.Push(obj);
    }
}
