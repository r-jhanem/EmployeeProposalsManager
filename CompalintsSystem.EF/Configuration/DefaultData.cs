using CompalintsSystem.Core.Helpers.Constants;
using CompalintsSystem.Core.Models;
using CompalintsSystem.EF.DataBase;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompalintsSystem.EF.Configuration
{
    public class DefaultData
    {
        public static async Task SeedCompalintAndSolustionAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppCompalintsContextDB>();

                if (!dbContext.UploadsComplaintes.Any())
                {
                    var uploadsCompalints = new UploadsComplainte
                    {

                        TitleComplaint = "تظلم بخصوص درجات الامتحانات",
                        DescComplaint = "\r\nالموضوع: تظلم بخصوص درجات الامتحانات \r\n\r\n\r\nأتمنى أن تصلكم هذه الرسالة في أتم الصحة والعافية. أنا سعيد علي احمد ، طالب يمني مخلص لهذا الوطن ومهتم بتطوير القطاع التعليمي في بلادنا الجميلة.\r\n\r\nأنا أقدم هذا التظلم بخصوص الدرجات التي حصلت عليها في الامتحانات الأخيرة. أشعر أن هناك خطأًا في تقييم الإجابات، وأن الدرجات لا تعكس أدائي الفعلي وفهمي للمواضيع.\r\n\r\nأطلب منكم مراجعة إجاباتي والنظر في التقييم مرة أخرى، حيث أعتقد أن هناك درجات غير مناسبة تمنحت لي. يرجى أخذ هذا التظلم بجدية واتخاذ الإجراءات اللازمة لمراجعة وتصحيح الدرجات بشكل عادل.\r\n\r\nأنا أدرك أن هذا يمكن أن يكون أمرًا معقدًا، ولكن أتوقع من الجامعة أن تضمن العدالة في التقييم وتوفير الفرصة للطلاب لتقديم تظلماتهم عند الحاجة.\r\n\r\nأنا مستعد لتقديم أي معلومات إضافية أو إجراء أي إجراءات تكميلية تسهم في فهم الوضع بشكل أفضل. أرجو النظر في هذا التظلم بروح من العدالة والشفافية.\r\n\r\nأشكركم مقدمًا على اهتمامكم وتفهمكم، وأتطلع إلى الرد في أقرب وقت ممكن.\r\n\r\nمع خالص التحية،",
                        CollegesId = 2,
                        DepartmentsId = 1,
                        SubDepartmentsId = 2,
                        FileName = "سعيد علي احمد",
                        UserId = "000243124222",
                        UploadDate = DateTime.Now,
                        TypeComplaintId = 1,
                        SocietyId = 1,
                        StatusCompalintId = 2,
                        StagesComplaintId = 1,
                        UserRoleName = UserRolesArbic.Beneficiarie
                    };
                    var compalintsSolution = new Compalints_Solution
                    {
                        ContentSolution = "اخي الطالب سعيد علي احمد \r\nتحية طيبة،\r\n\r\nلقد وصلتنا رسالتك المتعلقة بتظلمك بخصوص درجات الامتحانات، ونحن نفهم أهمية هذه القضية بالنسبة لك. سنقوم بفحص الوضع بعناية ومتابعة الإجراءات الضرورية لمراجعة وتصحيح الدرجات بشكل عادل.\r\n\r\nنأخذ تظلمات الطلاب على محمل الجد ونعمل جاهدين لضمان عدالة وشفافية عمليات التقييم. سنقوم بالتحقق من إجاباتك والتأكد من أن التقييم كان عادلاً وفقًا للمعايير المعتمدة.\r\n\r\nنحن نقدر تفهمك وصبرك في هذه القضية. سنعلمك بأي تطور في التحقيق ونتأكد من تقديم رد شافٍ وكامل خلال أقرب وقت ممكن.\r\n\r\nإذا كانت لديك أي معلومات إضافية أو تفاصيل ترغب في مشاركتها معنا، فيرجى الاتصال بنا. نحن هنا لدعمك وضمان حقوق الطلاب.\r\n\r\nشكرًا لتقديمك هذا التظلم. نحن نعمل بجدية لضمان جودة التعليم ورضا الطلاب، وسن",

                        DateSolution = DateTime.Now,
                        IsAccept = true,
                        UserId = "e70aa106-c6d6-4037-bb94-eb241dd1ef60",
                        SolutionProvName = "عبدالجليل سرحان غالب الدعيس",
                        UploadsComplainteId = 1,
                        SolutionProvIdentity = "001024312444",

                        Role = UserRolesArbic.AdminSubDepartments // "ادارة شئون الطلاب",


                    };

                    // await dbContext.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('UploadsComplainte', RESEED, 0)");//'UploadsComplainte' إلى القيمة '0' وتوليد القيمة '1' في المرة القادمة عند إدخال سجل جديد.
                    await dbContext.UploadsComplaintes.AddAsync(uploadsCompalints);
                    await dbContext.SaveChangesAsync();
                    await dbContext.Compalints_Solutions.AddAsync(compalintsSolution);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}