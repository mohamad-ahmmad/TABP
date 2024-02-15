namespace API.Models;
public class DiscountRequest
{
    /// <summary>
    /// Percentage like: 50, 20.5 this equals to 50%, 20.5%
    /// </summary>
    public double DiscountPercentage { get; set; }

    /// <summary>
    /// The date of discount will start (UTC)
    /// </summary>
    public DateTime FromDate { get; set; }


    /// <summary>
    /// The date of discount will end (UTC)
    /// </summary>
    public DateTime ToDate { get; set; }
}
