using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen_Recipe.RequestDTOs;

public class CreateRequest
{
    [FromBody]
    public PayPalCreateOrderRequestDto Body { get; set; }
    
    [FromServices]
    public PayPalService Paypal  { get; set; }
}

public class CaptureOrderRequest
{
    [FromBody]
    public PayPalCaptureOrderRequestDto Body { get; set; }
    
    [FromServices]
    public PayPalService Paypal  { get; set; }
}