using System;
using System.Collections.Generic;

public interface IResetable
{
    void Reset();
}

public abstract class Pool
{
    protected Stack<object> m_objectSatck;
    protected Action<object> m_resetAction;
    protected Action<object> m_onetimeInitAction;

    public abstract T Create<T>() where T : class,new();

    public abstract void Store(object obj);

    public abstract int GetPoolNum();
}

public class PoolNormal:Pool
{

    public PoolNormal(int initialBufferSize, Action<object> ResetAction = null, Action<object> OnetimeInitAction = null)
    {
        m_objectSatck = new Stack<object>(initialBufferSize);
        m_resetAction = ResetAction;
        m_onetimeInitAction = OnetimeInitAction;
    }
    public override T Create<T>() 
    { 
        if (m_objectSatck.Count > 0)
        {
            T t = m_objectSatck.Pop() as T;

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

    public override void Store(object obj)
    {
        m_objectSatck.Push(obj);
    }

    public override int GetPoolNum()
    {
        return m_objectSatck.Count;
    }
}

public class ResetPool<R> : Pool where R :class,IResetable,new() 
{
    public ResetPool(int initialBufferSize, Action<object> ResetAction = null, Action<object> OnetimeInitAction = null)
    {
        m_objectSatck = new Stack<object>(initialBufferSize);
        m_resetAction = ResetAction;
        m_onetimeInitAction = OnetimeInitAction;
    }

    public override T Create<T>()
    {
        if (m_objectSatck.Count > 0)
        {
            R r = m_objectSatck.Pop() as R ;
            r.Reset();

            if (m_resetAction != null)
            {
                m_resetAction(r);
            }
            return r as T;
        }
        else
        {
            R r = new R();
            if (m_onetimeInitAction != null)
            {
                m_onetimeInitAction(r);
            }
            return r as T;
        }
    }

    public override void Store(object obj)
    {
        m_objectSatck.Push(obj);
    }

    public override int GetPoolNum()
    {
        return m_objectSatck.Count;
    }
}



