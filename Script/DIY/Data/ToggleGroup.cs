using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIY.Data
{
	/// <summary>
	/// 模仿ToggleGroup的单选切换数据记录
	/// </summary>
	/// <typeparam name="T">一级标签</typeparam>
	/// <typeparam name="K">二级标签</typeparam>
	public class ToggleGroup<T, K> where T : class
	{
		private Dictionary<T, K> m_dic_toggles;
		private T m_current;

		public ToggleGroup()
		{
			m_dic_toggles = new Dictionary<T, K>();
		}

		public void Switch(T _target)
		{
			foreach (var item in m_dic_toggles)
			{
				if (item.Key.Equals(_target))
				{
					m_current = item.Key;
					break;
				}
			}
		}

		public void Switch(T _target, K _tag)
		{
			Switch(_target);
			AddOrSet(_target, _tag);
		}

		public void Close()
		{
			m_current = null;
		}

		public void AddOrSet(T _target, K _tag)
		{
			if (m_dic_toggles.ContainsKey(_target))
			{
				m_dic_toggles[_target] = _tag;
			}
			else
			{
				m_dic_toggles.Add(_target, _tag);
			}
		}

		public bool Check_ThisIsCurrent(T _target)
		{
			return m_current != null && m_current.Equals(_target);
		}

		public bool Check_ThisIsCurrent(T _target, K _tag)
		{
			if (Check_ThisIsCurrent(_target))
			{
				return m_dic_toggles[_target].Equals(_tag);
			}
			return false;
		}

		public K GetCurrentTag()
		{
			return m_dic_toggles[m_current];
		}
	}
}
