﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ContextGenerator.ttinclude code generation file.
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
	public partial class EntitiesModel : OpenAccessContext, IEntitiesModelUnitOfWork
	{
		private static string connectionStringName = @"CelloStoreConnection";
			
		private static BackendConfiguration backend = GetBackendConfiguration();
				
		private static MetadataSource metadataSource = XmlMetadataSource.FromAssemblyResource("EntitiesModel.rlinq");
		
		public EntitiesModel()
			:base(connectionStringName, backend, metadataSource)
		{ }
		
		public EntitiesModel(string connection)
			:base(connection, backend, metadataSource)
		{ }
		
		public EntitiesModel(BackendConfiguration backendConfiguration)
			:base(connectionStringName, backendConfiguration, metadataSource)
		{ }
			
		public EntitiesModel(string connection, MetadataSource metadataSource)
			:base(connection, backend, metadataSource)
		{ }
		
		public EntitiesModel(string connection, BackendConfiguration backendConfiguration, MetadataSource metadataSource)
			:base(connection, backendConfiguration, metadataSource)
		{ }
			
		public IQueryable<Tag> Tags 
		{
			get
			{
				return this.GetAll<Tag>();
			}
		}
		
		public IQueryable<Role> Roles 
		{
			get
			{
				return this.GetAll<Role>();
			}
		}
		
		public IQueryable<Product> Products 
		{
			get
			{
				return this.GetAll<Product>();
			}
		}
		
		public IQueryable<Person> People 
		{
			get
			{
				return this.GetAll<Person>();
			}
		}
		
		public IQueryable<PaymentMethod> PaymentMethods 
		{
			get
			{
				return this.GetAll<PaymentMethod>();
			}
		}
		
		public IQueryable<OrderStatusHistory> OrderStatusHistories 
		{
			get
			{
				return this.GetAll<OrderStatusHistory>();
			}
		}
		
		public IQueryable<OrderDetail> OrderDetails 
		{
			get
			{
				return this.GetAll<OrderDetail>();
			}
		}
		
		public IQueryable<Order> Orders 
		{
			get
			{
				return this.GetAll<Order>();
			}
		}
		
		public IQueryable<Configuration> Configurations 
		{
			get
			{
				return this.GetAll<Configuration>();
			}
		}
		
		public IQueryable<Category> Categories 
		{
			get
			{
				return this.GetAll<Category>();
			}
		}
		
		public IQueryable<Article> Articles 
		{
			get
			{
				return this.GetAll<Article>();
			}
		}
		
		public IQueryable<Area> Areas 
		{
			get
			{
				return this.GetAll<Area>();
			}
		}
		
		public IQueryable<AddressType> AddressTypes 
		{
			get
			{
				return this.GetAll<AddressType>();
			}
		}
		
		public IQueryable<Showcase> Showcases 
		{
			get
			{
				return this.GetAll<Showcase>();
			}
		}
		
		public IQueryable<AutoNumber> AutoNumbers 
		{
			get
			{
				return this.GetAll<AutoNumber>();
			}
		}
		
		public IQueryable<OrderStatus> OrderStatus 
		{
			get
			{
				return this.GetAll<OrderStatus>();
			}
		}
		
		public IQueryable<DeliveryTime> DeliveryTimes 
		{
			get
			{
				return this.GetAll<DeliveryTime>();
			}
		}
		
		public IQueryable<EmailTemplate> EmailTemplates 
		{
			get
			{
				return this.GetAll<EmailTemplate>();
			}
		}
		
		public IQueryable<PaymentConfirmation> PaymentConfirmations 
		{
			get
			{
				return this.GetAll<PaymentConfirmation>();
			}
		}
		
		public IQueryable<Bank> Banks 
		{
			get
			{
				return this.GetAll<Bank>();
			}
		}
		
		public static BackendConfiguration GetBackendConfiguration()
		{
			BackendConfiguration backend = new BackendConfiguration();
			backend.Backend = "MsSql";
			backend.ProviderName = "System.Data.SqlClient";
		
			CustomizeBackendConfiguration(ref backend);
		
			return backend;
		}
		
		/// <summary>
		/// Allows you to customize the BackendConfiguration of EntitiesModel.
		/// </summary>
		/// <param name="config">The BackendConfiguration of EntitiesModel.</param>
		static partial void CustomizeBackendConfiguration(ref BackendConfiguration config);
		
	}
	
	public interface IEntitiesModelUnitOfWork : IUnitOfWork
	{
		IQueryable<Tag> Tags
		{
			get;
		}
		IQueryable<Role> Roles
		{
			get;
		}
		IQueryable<Product> Products
		{
			get;
		}
		IQueryable<Person> People
		{
			get;
		}
		IQueryable<PaymentMethod> PaymentMethods
		{
			get;
		}
		IQueryable<OrderStatusHistory> OrderStatusHistories
		{
			get;
		}
		IQueryable<OrderDetail> OrderDetails
		{
			get;
		}
		IQueryable<Order> Orders
		{
			get;
		}
		IQueryable<Configuration> Configurations
		{
			get;
		}
		IQueryable<Category> Categories
		{
			get;
		}
		IQueryable<Article> Articles
		{
			get;
		}
		IQueryable<Area> Areas
		{
			get;
		}
		IQueryable<AddressType> AddressTypes
		{
			get;
		}
		IQueryable<Showcase> Showcases
		{
			get;
		}
		IQueryable<AutoNumber> AutoNumbers
		{
			get;
		}
		IQueryable<OrderStatus> OrderStatus
		{
			get;
		}
		IQueryable<DeliveryTime> DeliveryTimes
		{
			get;
		}
		IQueryable<EmailTemplate> EmailTemplates
		{
			get;
		}
		IQueryable<PaymentConfirmation> PaymentConfirmations
		{
			get;
		}
		IQueryable<Bank> Banks
		{
			get;
		}
	}
}
#pragma warning restore 1591
