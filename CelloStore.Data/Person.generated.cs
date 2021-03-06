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
	public partial class Person
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
		
		private string email;
		public virtual string Email
		{
			get
			{
				return this.email;
			}
			set
			{
				this.email = value;
			}
		}
		
		private string phoneNumber;
		public virtual string PhoneNumber
		{
			get
			{
				return this.phoneNumber;
			}
			set
			{
				this.phoneNumber = value;
			}
		}
		
		private string password;
		public virtual string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}
		
		private string address;
		public virtual string Address
		{
			get
			{
				return this.address;
			}
			set
			{
				this.address = value;
			}
		}
		
		private string city;
		public virtual string City
		{
			get
			{
				return this.city;
			}
			set
			{
				this.city = value;
			}
		}
		
		private string state;
		public virtual string State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}
		
		private string postalCode;
		public virtual string PostalCode
		{
			get
			{
				return this.postalCode;
			}
			set
			{
				this.postalCode = value;
			}
		}
		
		private System.Nullable<System.Char> gender;
		public virtual System.Nullable<System.Char> Gender
		{
			get
			{
				return this.gender;
			}
			set
			{
				this.gender = value;
			}
		}
		
		private DateTime? birthDate;
		public virtual DateTime? BirthDate
		{
			get
			{
				return this.birthDate;
			}
			set
			{
				this.birthDate = value;
			}
		}
		
		private bool isActive;
		public virtual bool IsActive
		{
			get
			{
				return this.isActive;
			}
			set
			{
				this.isActive = value;
			}
		}
		
		private bool isVerified;
		public virtual bool IsVerified
		{
			get
			{
				return this.isVerified;
			}
			set
			{
				this.isVerified = value;
			}
		}
		
		private int roleId;
		public virtual int RoleId
		{
			get
			{
				return this.roleId;
			}
			set
			{
				this.roleId = value;
			}
		}
		
		private string challangeCode;
		public virtual string ChallangeCode
		{
			get
			{
				return this.challangeCode;
			}
			set
			{
				this.challangeCode = value;
			}
		}
		
		private string lastIpAddress;
		public virtual string LastIpAddress
		{
			get
			{
				return this.lastIpAddress;
			}
			set
			{
				this.lastIpAddress = value;
			}
		}
		
		private DateTime? lastLogOn;
		public virtual DateTime? LastLogOn
		{
			get
			{
				return this.lastLogOn;
			}
			set
			{
				this.lastLogOn = value;
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
		
		private DateTime? activatedWhen;
		public virtual DateTime? ActivatedWhen
		{
			get
			{
				return this.activatedWhen;
			}
			set
			{
				this.activatedWhen = value;
			}
		}
		
		private DateTime? challangedWhen;
		public virtual DateTime? ChallangedWhen
		{
			get
			{
				return this.challangedWhen;
			}
			set
			{
				this.challangedWhen = value;
			}
		}
		
		private DateTime? lastChangePassword;
		public virtual DateTime? LastChangePassword
		{
			get
			{
				return this.lastChangePassword;
			}
			set
			{
				this.lastChangePassword = value;
			}
		}
		
		private string lastName;
		public virtual string LastName
		{
			get
			{
				return this.lastName;
			}
			set
			{
				this.lastName = value;
			}
		}
		
		private string firstName;
		public virtual string FirstName
		{
			get
			{
				return this.firstName;
			}
			set
			{
				this.firstName = value;
			}
		}
		
		private Role role;
		public virtual Role Role
		{
			get
			{
				return this.role;
			}
			set
			{
				this.role = value;
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
		
	}
}
#pragma warning restore 1591
