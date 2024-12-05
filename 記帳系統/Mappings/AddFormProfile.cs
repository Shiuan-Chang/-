using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 記帳系統.Models;
using AutoMapper;
using 記帳系統.Models;
using System.Drawing;

namespace 記帳系統.Mappings
{
    public class AddFormProfile : Profile
    {
        public AddFormProfile()
        {
            // AddModel -> AddFormRawDataDTO
            CreateMap<AddModel, AddFormRawDataDTO>()
              .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.SelectedAccountName))
              .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.SelectedAccountType))
              .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Detail))
              .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment))
              .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
              .ForMember(dest => dest.Picture1, opt => opt.MapFrom(src => src.Picture1)) // 保留圖片對象
              .ForMember(dest => dest.Picture2, opt => opt.MapFrom(src => src.Picture2)); // 保留圖片對象
            // AddFormRawDataDTO -> AddFormRawDataDAO
            CreateMap<AddFormRawDataDTO, AddFormRawDataDAO>()
              .ForMember(dest => dest.Picture1Path, opt => opt.Ignore()) // 路徑由外部保存邏輯處理
              .ForMember(dest => dest.Picture2Path, opt => opt.Ignore()); // 路徑由外部保存邏輯處理
        }
    }



}
