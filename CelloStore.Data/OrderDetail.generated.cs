#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ClassGenerator.ttinclude code generation file.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using CelloStore.Data;

namespace CelloStore.Data	
{
	public partial class OrderDetail
	{
		private int id;
		public virtual int Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		
		private int orderId;
		public virtual int OrderId
		{
			get
			{
				return this.orderId;
			}
			set
			{
				this.orderId = value;
			}
		}
		
		private int productId;
		public virtual int ProductId
		{
			get
			{
				return this.productId;
			}
			set
			{
				this.productId = value;
			}
		}
		
		private int qty;
		public virtual int Qty
		{
			get
			{
				return this.qty;
			}
			set
			{
				this.qty = value;
			}
		}
		
		private decimal unitPrice;
		public virtual decimal UnitPrice
		{
			get
			{
				return this.unitPrice;
			}
			set
			{
				this.unitPrice = value;
			}
		}
		
		private decimal discValue;
		public virtual decimal DiscValue
		{
			get
			{
				return this.discValue;
			}
			set
			{
				this.discValue = value;
			}
		}
		
		private Product product;
		public virtual Product Product
		{
			get
			{
				return this.product;
			}
			set
			{
				this.product = value;
			}
		}
		
		private Order order;
		public virtual Order Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		
	}
}
#pragma warning restore 1591
