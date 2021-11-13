// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Karma.Models;

namespace Karma.Pages
{
    public partial class Index
    {
        public string filterValue = "";

        private KarmaContext m_karmaContext = new KarmaContext();

        public IEnumerable<IGenericKarmaItem> GetEvents()
        {
            var result = m_karmaContext.Events.ToList();
            result.Sort();
            return result;
        }

        public void NavigateToIndividualEvent(Guid id)
        {
            m_uriHelper.NavigateTo($"event/{id}");
        }
    }
}
