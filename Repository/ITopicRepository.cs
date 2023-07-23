using BussinessObject.DTOs;
using BussinessObject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ITopicRepository
    {
        void addTopic(Topics topic);
        void deleteTopic(int id);
        void updateTopic(Topics topic);
        Topics getTopic(int id);
        List<Topics> getTopicsByCourseId(int id);
        Task addMeterial(IFormFile fileData,int TopicId);
        Task dowloadMeterial(int id);

    }
}
