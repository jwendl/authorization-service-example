namespace PolicyManager.DataAccess.Models
{
    public class PolicyResult
    {
        /// <summary>
        /// The name of the policy.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the policy.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The result either Allow or Deny.
        /// </summary>
        public PolicyEvaluation Result { get; set; }

        /// <summary>
        /// The result in string format.
        /// </summary>
        public string ResultString
        {
            get
            {
                return Result.ToString();
            }
        }
    }

    public enum PolicyEvaluation
    {
        Deny,
        Allow,
    }
}
