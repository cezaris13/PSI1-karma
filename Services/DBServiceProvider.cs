// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Karma.Models;
using MatBlazor;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace Karma.Services
{
    public class DBServiceProvider : IDBServiceProvider
    {
        public KarmaContext karmaContext { get; set; }

        private readonly INotifactionTransmitter m_notifactionTransmitter;
        private readonly IObjectChecker m_objectChecker;

        public DBServiceProvider(
            INotifactionTransmitter notifactionTransmitter,
            IDbContextFactory<KarmaContext> karmaContextFactory,
            IObjectChecker objectChecker)
        {
            m_notifactionTransmitter = notifactionTransmitter;
            karmaContext = karmaContextFactory.CreateDbContext();
            m_objectChecker = objectChecker;
        }

        public int AddToDB<T>(T entity) where T : class
        {
            IEnumerable<PropertyInfo> result = karmaContext.GetType().GetProperties().Where(p => p.PropertyType == typeof(DbSet<T>));
            if (!result.Any())
            {
                return -1;
            }
            var dbSet = result.First().GetValue(karmaContext) as DbSet<T>;
            try
            {
                dbSet.Add(entity);
                karmaContext.SaveChanges();
                m_notifactionTransmitter.ShowMessage($"The {ReturnTypeName<T>()} has been created", MatToastType.Success);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public int RemoveFromDB<T>(Guid id) where T : class
        {
            IEnumerable<PropertyInfo> result = karmaContext.GetType().GetProperties().Where(p => p.PropertyType == typeof(DbSet<T>));
            if (!result.Any())
            {
                return -1;
            }
            var dbSet = result.First().GetValue(karmaContext) as DbSet<T>;
            try
            {
                karmaContext.Events.Where(x => x.Id == id).Delete();
                karmaContext.SaveChanges();
                m_notifactionTransmitter.ShowMessage($"The {ReturnTypeName<T>()} has been deleted", MatToastType.Success);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        public int UpdateEntityInDB<T>(T entity) where T : class
        {
            if (m_objectChecker.IsAnyNullOrEmpty(entity))
            {
                m_notifactionTransmitter.ShowMessage("There are some empty fields", MatToastType.Danger);
                return -2;
            }
            IEnumerable<PropertyInfo> result = karmaContext.GetType().GetProperties().Where(p => p.PropertyType == typeof(DbSet<T>));
            if (!result.Any())
            {
                return -1;
            }
            var dbSet = result.First().GetValue(karmaContext) as DbSet<T>;
            try
            {
                dbSet.Update(entity);
                karmaContext.SaveChanges();
                m_notifactionTransmitter.ShowMessage($"The {ReturnTypeName<T>()} has been updated", MatToastType.Success);
                return 0;
            }
            catch (DbUpdateException)
            {
                return -1;
            }
        }

        private string ReturnTypeName<T>()
        {
            return Regex.Replace(typeof(T).Name, "([a-z])([A-Z])", "$1 $2").ToLower();
        }
    }
}
