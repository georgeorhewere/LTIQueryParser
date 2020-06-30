using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    public class ResourceSet
    {
        public int id { set; get; }
        public string name { set; get; }
        public string description { set; get; }        
        public string url { set; get; }               
        
        public string author { set; get; }
        public string publisher { set; get; }
        
    }

    public class ResourceList
    {

        public static IQueryable<ResourceSet> GetList()
        {
            return new List<ResourceSet> { 
                new ResourceSet
                {
                    id = 1,
                    name="Bingo Blitz",
                    description="Students do math problems and find answers on their bingo cards. This game is described for decimal computation, but can be used for any kinds of computations and practicing fluent recall of math skills.",
                    url="http://www.lessonplanspage.com/MathDecimalAddSubBingoBlitzIdea34.htm",
                    publisher="Abbie Artley"
                },
                new ResourceSet
                {
                    id = 2,
                    name="Magnificent Measurement: The Weight of Things",
                    description="This  lesson  introduces  students  to  weight,  first  by  having  them  predict  which  objects  are  heaviest  of  various  sets  of  objects.  Students  are  encouraged  to  explain  how  they  determined  which  object  was  heavier,  while  being  guided  to  use  mathematical  terms.  Next,  scales  are  employed  to  weigh  objects",
                    url="http://illuminations.nctm.org/Lesson.aspx?id=713",
                    publisher="Illuminations National Council of Teachers of Mathematics"
                },
                new ResourceSet
                {
                    id = 3,
                    name="Shodor Interactivate",
                    description="The goals of Interactivate are the creation, collection, evaluation, and dissemination of interactive Java-based courseware for exploration in science and mathematics.",
                    url="http://www.shodor.org/interactivate/lessons",
                    publisher="Shodor Interactivate"
                },
                new ResourceSet
                {
                    id = 4,
                    name="Regents Prep",
                    description="This website has resources for the Regents Exam, which is similar to FCAT.  Math A | Math B | Algebra | Geometry | Algebra2/Trig | Global History | U.S. History | Earth Science ",
                    url="http://www.regentsprep.org",
                    publisher="Oswego City School District Regents Exam Prep Ct"
                },
                new ResourceSet
                {
                    id = 5,
                    name="The Futures Channel",
                    description="A multi-media website designed to connect mathematics, science, technology and engineering to the real world of careers and achievement, providing context and purpose for what students are learning, allowing then to envision their own successful futures",
                    url="http://thefutureschannel.com/hands-on_math.php",
                    publisher="The Futures Channel"
                },


            }.AsQueryable();
        }
    }
}
