namespace TF.Providers
{
	public class Azure : Provider
	{
		[Name("ARM_CLIENT_ID")]
		public string ClientId { get; set; }
		[Name("ARM_CLIENT_SECRET")]
		public string ClientSecret { get; set; }
		[Name("ARM_SUBSCRIPTION_ID")]
		public string SubscriptionId { get; set; }
		[Name("ARM_TENANT_ID")]
		public string TenantId { get; set; }
		[Name("ARM_USE_MSI", Lower = true)]
		public bool UseMsi { get; set; }
		public Azure()
		{
			this.UseMsi = false;
		}
	}
}