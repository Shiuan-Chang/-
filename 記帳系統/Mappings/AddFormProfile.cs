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
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // AddModel -> AddFormRawDataDTO
            CreateMap<AddModel, AddFormRawDataDTO>()
              .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.SelectedAccountName))
              .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.SelectedAccountType))
              .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Detail))
              .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payment))
              .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
              .ForMember(dest => dest.Picture1, opt => opt.MapFrom(src => src.Picture1)) 
              .ForMember(dest => dest.Picture2, opt => opt.MapFrom(src => src.Picture2)); 
            // AddFormRawDataDTO -> AddFormRawDataDAO
            CreateMap<AddFormRawDataDTO, AddFormRawDataDAO>()
              .ForMember(dest => dest.Picture1Path, opt => opt.Ignore()) 
              .ForMember(dest => dest.Picture2Path, opt => opt.Ignore()) 
              .ForMember(dest => dest.CompressedPicture1Path, opt => opt.Ignore()) 
              .ForMember(dest => dest.CompressedPicture2Path, opt => opt.Ignore()); 
            // NoteFormRawDataDAO -> NoteModel
            CreateMap<NotFormRawDataDAO, NoteModel>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType))
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Detail))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.originalPicture1Path, opt => opt.MapFrom(src => src.Picture1Path))
                .ForMember(dest => dest.originalPicture2Path, opt => opt.MapFrom(src => src.Picture2Path))
                .ForMember(dest => dest.CompressedPicture1Path, opt => opt.MapFrom(src => src.CompressedPicture1Path))
                .ForMember(dest => dest.CompressedPicture2Path, opt => opt.MapFrom(src => src.CompressedPicture2Path));
            CreateMap<NotFormRawDataDAO, NotFormRawDataDTO>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Detail))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Picture1Path, opt => opt.MapFrom(src => src.Picture1Path))
                .ForMember(dest => dest.Picture2Path, opt => opt.MapFrom(src => src.Picture2Path))
                .ForMember(dest => dest.CompressedPicture1Path, opt => opt.MapFrom(src => src.CompressedPicture1Path))
                .ForMember(dest => dest.CompressedPicture2Path, opt => opt.MapFrom(src => src.CompressedPicture2Path));
            // AccountRawDataDAO -> AccountModel
            CreateMap<AccountRawDataDAO, AccountModel>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType))
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Detail))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
        }
    }
}
