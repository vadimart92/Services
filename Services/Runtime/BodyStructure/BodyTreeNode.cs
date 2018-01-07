namespace Services.Runtime.BodyStructure
{
	using System.Collections.ObjectModel;
	using System.Linq;
	using Services.Metadata;

	public class BodyTree: BodyTreeNode
	{

	}

	public class BodyTreeArray : Collection<IBodyTreeObject>, IBodyTreeItem
	{

		public ServiceParameter Parameter { get; set; }

	}

	public class BodyTreeObject : BodyTreeNode, IBodyTreeObject
	{

		public object Value => this.ToList();

	}
	public class BodyTreeNode:Collection<IBodyTreeItem>, IBodyTreeItem
	{
		public ServiceParameter Parameter { get; set; }
	}

	public interface IBodyTreeItem
	{

		ServiceParameter Parameter { get; }

	}

	public interface IBodyTreeObject: IBodyTreeItem
	{
		object Value { get; }
	}

	public class BodyTreeNodeParameterValue : IBodyTreeObject
	{
		public ServiceParameter Parameter { get; set; }

		public object Value { get; set; }

	}
}
