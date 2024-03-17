using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Mango.Services.CouponAPI.Controllers;
[Route("api/coupon")]
[ApiController]
[Authorize]
public class CouponAPIController : ControllerBase
{
    private readonly AppDbContext _db;
    private ResponseDto _responseDto;
    private readonly IMapper _mapper;

    public CouponAPIController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
        _responseDto = new ResponseDto();
    }

    [HttpGet]
    public ResponseDto Get()
    {
        try
        {
            var objList = _db.Coupons.ToList();
            _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
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
            _responseDto.Result = _mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }

    [HttpGet]
    [Route("GetByCode/{code}")]
    public object? GetByCode(string code)
    {
        try
        {
            var obj = _db.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
            _responseDto.Result = _mapper.Map<CouponDto>(obj);
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
     }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto Post([FromBody] CouponDto couponDto)
    {

        try
        {
            var coupon = _mapper.Map<Coupon>(couponDto);
            _db.Coupons.Add(coupon);
            _db.SaveChanges();
            _responseDto.Result = _mapper.Map<CouponDto>(coupon);
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }


    [HttpPut]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto Put([FromBody] CouponDto couponDto)
    {
        try
        {
            var coupon = _mapper.Map<Coupon>(couponDto);
            _db.Coupons.Update(coupon);
            _db.SaveChanges();
            _responseDto.Result = _mapper.Map<CouponDto>(coupon);
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public ResponseDto Delete(int id)
    {

        try
        {
            var coupon = _db.Coupons.Find(id);  
            if(coupon is not null)
            {
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = ex.Message;
        }

        return _responseDto;
    }
}

