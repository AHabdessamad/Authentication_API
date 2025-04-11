using AutoMapper;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
        }       


    }
}
