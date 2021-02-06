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
	public partial class PaymentMethod
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
		
		private string name;
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		
		private string accountNumber;
		public virtual string AccountNumber
		{
			get
			{
				return this.accountNumber;
			}
			set
			{
				this.accountNumber = value;
			}
		}
		
		private string accountName;
		public virtual string AccountName
		{
			get
			{
				return this.accountName;
			}
			set
			{
				this.accountName = value;
			}
		}
		
		private string notes;
		public virtual string Notes
		{
			get
			{
				return this.notes;
			}
			set
			{
				this.notes = value;
			}
		}
		
		private DateTime createdWhen;
		public virtual DateTime CreatedWhen
		{
			get
			{
				return this.createdWhen;
			}
			set
			{
				this.createdWhen = value;
			}
		}
		
		private string createdBy;
		public virtual string CreatedBy
		{
			get
			{
				return this.createdBy;
			}
			set
			{
				this.createdBy = value;
			}
		}
		
		private DateTime changedWhen;
		public virtual DateTime ChangedWhen
		{
			get
			{
				return this.changedWhen;
			}
			set
			{
				this.changedWhen = value;
			}
		}
		
		private string changedBy;
		public virtual string ChangedBy
		{
			get
			{
				return this.changedBy;
			}
			set
			{
				this.changedBy = value;
			}
		}
		
		private IList<Order> orders = new List<Order>();
		public virtual IList<Order> Orders
		{
			get
			{
				return this.orders;
			}
		}
		
		private IList<PaymentConfirmation> paymentConfirmations = new List<PaymentConfirmation>();
		public virtual IList<PaymentConfirmation> PaymentConfirmations
		{
			get
			{
				return this.paymentConfirmations;
			}
		}
		
	}
}
#pragma warning restore 1591
