using BussinessObject.DTOs;
using BussinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TopicRepository : ITopicRepository
    {
        public void addMeterial()
        {
            throw new NotImplementedException();
        }

        public void addTopic(Topics topic) => TopicDAO.addTopic(topic);

        public void deleteTopic(int id) => TopicDAO.deleteTopic(id);

        public Topics getTopic(int id) => TopicDAO.getTopicById(id);

        public List<Topics> getTopicsByCourseId(int id) => TopicDAO.getTopicsByCourseId(id);
         
        public void updateTopic(Topics topic) => TopicDAO.updateTopic(topic);

        public Task addMeterial(IFormFile fileData,int topicId) => TopicDAO.addMeterial(fileData, topicId);
        public Task dowloadMeterial(int id) => TopicDAO.dowloadMeterial(id);
    }
}
