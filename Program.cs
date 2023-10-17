// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Samples.Common;
using Azure.ResourceManager.Compute;

namespace ListComputeSkus
{
    /**
     * Azure Compute sample for managing Compute SKUs -
     *  - List all compute SKUs in the subscription
     *  - List compute SKUs for a specific compute resource type (VirtualMachines) in a region
     *  - List compute SKUs for a specific compute resource type (Disks).
     */
    public class Program
    {
        public static async Task RunSample(ArmClient client)
        {
            //=================================================================

            // List all compute SKUs in the subscription
            var subscription = await client.GetDefaultSubscriptionAsync();
            var skus = subscription.GetComputeResourceSkus();
            Utilities.Log("Listing Compute SKU in the subscription");
            var format = "{0,-22} {1,-22} {2,-22} {3}";
            Utilities.Log(String.Format(format, "Name", "ResourceType", "Size", "Regions [zones]"));
            Utilities.Log("========================================================================================");
            var hashSet = new HashSet<string>();
            foreach (var sku in skus)
            {
                String size = null;
                String regionZones = null;
                if (sku.ResourceType.Equals("virtualMachines"))
                {
                    size = sku.Size;
                }
                else if (sku.ResourceType.Equals("availabilitySets"))
                {
                    size = sku.Size;
                }
                else if (sku.ResourceType.Equals("disks"))
                {
                    size = sku.Size;
                }
                else if (sku.ResourceType.Equals("snapshots"))
                {
                    size = sku.Size;
                }
                if (sku != null && sku.LocationInfo != null && sku.LocationInfo.Count > 0)
                {
                    regionZones = sku.Locations[0].ToString() + ":"+ string.Join(",", sku.LocationInfo[0].Zones);
                }
                else
                {
                    regionZones = null;
                }
                Utilities.Log(String.Format(format, sku.Name, sku.ResourceType, size, regionZones));
            }

            //=================================================================

            // List compute SKUs for a specific compute resource type (VirtualMachines) in a region
            Utilities.Log("Listing compute SKUs for a specific compute resource type (VirtualMachines) in a region (US East2)");
            format = "{0,-22} {1,-22} {2}";
            Utilities.Log(String.Format(format, "Name", "Size", "Regions [zones]"));
            Utilities.Log("============================================================================");
            var skuss = subscription.GetComputeResourceSkus();
            foreach(var sku in skuss) 
            {
                String regionZones = null;
                if (sku.ResourceType.Equals("virtualMachines") && sku.Locations[0].ToString().Equals("eastus2"))
                {
                    if (sku != null && sku.LocationInfo != null && sku.LocationInfo.Count > 0)
                    {
                        regionZones = sku.Locations[0].ToString() + ":" + string.Join(",", sku.LocationInfo[0].Zones);
                    }
                    else
                    {
                        regionZones = null;
                    }
                    var line = String.Format(format, sku.Name, sku.Size, regionZones);
                    Utilities.Log(line);
                }
            }

            //=================================================================

            // List compute SKUs for a specific compute resource type (Disks)
            Utilities.Log("Listing compute SKUs for a specific compute resource type (Disks)");
            format = "{0,-22} {1,-22} {2}";
            Utilities.Log(String.Format(format, "Name", "Type", "Regions [zones]"));
            Utilities.Log("============================================================================");
            foreach(var sku in skus)
            {
                String regionZones = null;
                if (sku.ResourceType.Equals("disks"))
                {
                    if (sku != null && sku.LocationInfo != null && sku.LocationInfo.Count > 0)
                    {
                        regionZones = sku.Locations[0].ToString() + ":" + string.Join(",", sku.LocationInfo[0].Zones);
                    }
                    else
                    {
                        regionZones = null;
                    }
                    var line = String.Format(format, sku.Name, sku.ResourceType, regionZones);
                    Utilities.Log(line);
                }
            }
        }
        public static async Task Main(string[] args)
        {
            try
            {
                var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
                var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
                var subscription = Environment.GetEnvironmentVariable("SUBSCRIPTION_ID");
                ClientSecretCredential credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                ArmClient client = new ArmClient(credential, subscription);
                await RunSample(client);
            }
            catch (Exception e)
            {
                Utilities.Log(e);
            }
        }
    }
}