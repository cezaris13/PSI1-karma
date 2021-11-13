﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;

namespace Karma.Services
{
    public interface IDBServiceProvider
    {
        int AddToDB<T>(T entity) where T : class;
        int RemoveFromDB<T>(Guid entity) where T : class;
        int UpdateEntityInDB<T>(T entity) where T : class;
    }
}
