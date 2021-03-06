﻿//-----------------------------------------------------------------------
// <copyright file="IngestManifestFileData.cs" company="Microsoft">Copyright 2012 Microsoft Corporation</copyright>
// <license>
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </license>

using System;
using System.Data.Services.Client;
using System.Data.Services.Common;
using System.Threading.Tasks;

namespace Microsoft.WindowsAzure.MediaServices.Client
{
    [DataServiceKey("Id")]
    internal partial class IngestManifestFileData : IIngestManifestFile, ICloudMediaContextInit
    {
        private CloudMediaContext _cloudMediaContext;
        internal string Path = String.Empty;

        public IngestManifestFileData()
        {
            Id = string.Empty;
        }

        #region ICloudMediaContextInit Members

        public void InitCloudMediaContext(CloudMediaContext context)
        {
            _cloudMediaContext = context;
        }

        #endregion

        #region IManifestAssetFile Members

        /// <summary>
        /// Deletes the manifest asset file asynchronously.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public Task DeleteAsync()
        {
            DataServiceContext dataContext = _cloudMediaContext.DataContextFactory.CreateDataServiceContext();
            dataContext.AttachTo(IngestManifestFileCollection.EntitySet, this);
            dataContext.DeleteObject(this);
            return dataContext.SaveChangesAsync(this);
        }

        /// <summary>
        /// Deletes manifest asset fils synchronously.
        /// </summary>
        public void Delete()
        {
            try
            {
                DeleteAsync().Wait();
            }
            catch (AggregateException exception)
            {
                throw exception.InnerException;
            }
        }

        #endregion

        private static IngestManifestFileState GetExposedState(int state)
        {
            return (IngestManifestFileState) state;
        }
    }
}