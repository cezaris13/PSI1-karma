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
        private readonly INotifactionTransmitter m_notifactionTransmitter;
        private readonly KarmaContext m_karmaContext;
        private readonly IObjectChecker m_objectChecker;

        public DBServiceProvider(
            INotifactionTransmitter notifactionTransmitter,
            IKarmaContextFactory karmaContextFactory,
            IObjectChecker objectChecker)
        {
            m_notifactionTransmitter = notifactionTransmitter;
            m_karmaContext = karmaContextFactory.CreateKarmaContext();
            m_objectChecker = objectChecker;
        }

        public int AddToDB<T>(T entity) where T : class
        {
            IEnumerable<PropertyInfo> result = m_karmaContext.GetType().GetProperties().Where(p => p.PropertyType == typeof(DbSet<T>));
            if (!result.Any())
            {
                return -1;
            }
            var dbSet = result.First().GetValue(m_karmaContext) as DbSet<T>;
            try
            {
                dbSet.Add(entity);
                m_karmaContext.SaveChanges();
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
            IEnumerable<PropertyInfo> result = m_karmaContext.GetType().GetProperties().Where(p => p.PropertyType == typeof(DbSet<T>));
            if (!result.Any())
            {
                return -1;
            }
            var dbSet = result.First().GetValue(m_karmaContext) as DbSet<T>;
            try
            {
                m_karmaContext.Events.Where(x => x.Id == id).Delete();
                m_karmaContext.SaveChanges();
                m_notifactionTransmitter.ShowMessage($"The {ReturnTypeName<T>()}) has been deleted", MatToastType.Success);
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
            IEnumerable<PropertyInfo> result = m_karmaContext.GetType().GetProperties().Where(p => p.PropertyType == typeof(DbSet<T>));
            if (!result.Any())
            {
                return -1;
            }
            var dbSet = result.First().GetValue(m_karmaContext) as DbSet<T>;
            try
            {
                dbSet.Update(entity);
                m_karmaContext.SaveChanges();
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
