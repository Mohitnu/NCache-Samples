﻿// ===============================================================================
// Alachisoft (R) NCache Sample Code.
// ===============================================================================
// Copyright © Alachisoft.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===============================================================================

using System;
using System.Collections;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Client;
using Alachisoft.NCache.Sample.Data;
using System.Configuration;

namespace Alachisoft.NCache.Samples
{
    /// <summary>
    /// Class that demonstrates the usage of NamedTags in NCache.
    /// </summary>
    public class NamedTags
    {
        private static ICache _cache;

        /// <summary>
        /// Executing this method will perform the operations of the sample using Named tags.
        /// </summary>
        public static void Run()
        {
            // Initialize cache
            InitializeCache();

            // Creating named tag dictionary. 
            NamedTagsDictionary namedTagDict = new NamedTagsDictionary();
            namedTagDict.Add("Category", "Beverages");
            namedTagDict.Add("ProductName", "Coke");

            // Add Items in cache with named tags
            AddItems(namedTagDict);

            // Fetch Items from the cache
            GetItems();

            // Dispose cache once done
            _cache.Dispose();
        }

        /// <summary>
        /// This method initializes the cache.
        /// </summary>
        private static void InitializeCache()
        {
            string cache = ConfigurationManager.AppSettings["CacheId"];

            if (String.IsNullOrEmpty(cache))
            {
                Console.WriteLine("The Cache Name cannot be null or empty.");
                return;
            }

            // Initialize an instance of the cache to begin performing operations:
            _cache = NCache.Client.CacheManager.GetCache(cache);

            Console.WriteLine("Cache initialized successfully");
        }

        /// <summary>
        /// This method adds items in the cache along with namedTags.
        /// </summary>
        /// <param name="namedTagDict"> Named tags that will be added with the items. </param>
        private static void AddItems(NamedTagsDictionary namedTagDict)
        {
            // Add Name Tags data to Cache
            AddNamedTagDataToCache(14, "Andrew Ng", "USA", namedTagDict);
            AddNamedTagDataToCache(15, "Jhon Getsby", "France", namedTagDict);

            Console.WriteLine("Items added in cache.");
        }
        /// <summary>
        /// Add Named Tags data to Cache
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productName"></param>
        /// <param name="group"></param>
        /// <param name="className"></param>
        /// <param name="category"></param>
        private static void AddNamedTagDataToCache(int orderID, string shipName, string shipCountry, NamedTagsDictionary namedTagDict)
        {
            Order order = new Order() { OrderID = orderID, ShipName = shipName, ShipCountry = shipCountry };

            // create object key
            string key = "Order:" + order.OrderID.ToString();

            // create CacheItem with your desired object
            CacheItem item = new CacheItem(order);

            // assign NameTags to CacheItem object
            item.NamedTags = namedTagDict;

            // add CacheItem object to cache
            _cache.Add(key, item);
        }

        /// <summary>
        /// This method fetches items from the cache using named tags.
        /// </summary>
        private static void GetItems()
        {
            string query = "SELECT Alachisoft.NCache.Sample.Data.Order WHERE this.Category = ? AND this.ProductName = ?";

            QueryCommand itemQuery = new QueryCommand(query);
            itemQuery.Parameters.Add("Category", "Beverages");
            itemQuery.Parameters.Add("ProductName", "Coke");

            ICacheReader result = _cache.SearchService.ExecuteReader(itemQuery, true);

            if (!result.IsClosed)
            {
                while (result.Read())
                {
                    Console.WriteLine(result.GetString(0));
                }
            }
        }
    }
}