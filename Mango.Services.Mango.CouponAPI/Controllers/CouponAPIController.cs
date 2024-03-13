using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Mango.Services.CouponAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CouponAPIController : ControllerBase
{
    private readonly AppDbContext _db;
    private ResponseDto _responseDto;

    public CouponAPIController(AppDbContext db)
    {
        _db = db;
        _responseDto = new ResponseDto();
    }

    [HttpGet]
    public ResponseDto Get()
    {
        try
        {
            var objList = _db.Coupons.ToList();
            _responseDto.Result = objList;
            return _responseDto;
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }

    [HttpGet]
    [Route("{id:int}")]
    public object? Get(int id)
    {
        try
        {
            var obj = _db.Coupons.Find(id);
            _responseDto.Result = obj;
            return _responseDto;
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }
}
