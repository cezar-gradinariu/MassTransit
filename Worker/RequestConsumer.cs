using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Worker.Interfaces;

namespace Worker
{
    public class RequestConsumer : RequestConsumerBase<CurrencyRequest, CurrencyResponse>
    {
        public RequestConsumer(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }

        protected override async Task ConsumeRequest(ConsumeContext<CurrencyRequest> context)
        {
            Logger.Information("Start consuming the request");
            Logger.Error("Emulated error call with id {callId} is done on worker.");
            for (int i = 0; i < 1; i++)
            {
                Logger.Information("LogEntry,'Message','0','Received SpotRateHistoryResponse reply: {''CurrentInterbankRate'':2.0386219260405196494019813686,''CurrentInverseInterbankRate'':0.490527442693718972587955968,''HistoricalPoints'':[{''PointInTime'':1464570300000,''InterbankRate'':''2.038783'',''InverseInterbankRate'':''0.490489''},{''PointInTime'':1464570900000,''InterbankRate'':''2.038389'',''InverseInterbankRate'':''0.490584''},{''PointInTime'':1464571500000,''InterbankRate'':''2.041913'',''InverseInterbankRate'':''0.489737''},{''PointInTime'':1464572100000,''InterbankRate'':''2.041382'',''InverseInterbankRate'':''0.489864''},{''PointInTime'':1464572700000,''InterbankRate'':''2.040055'',''InverseInterbankRate'':''0.490183''},{''PointInTime'':1464573300000,''InterbankRate'':''2.039832'',''InverseInterbankRate'':''0.490237''},{''PointInTime'':1464573900000,''InterbankRate'':''2.039975'',''InverseInterbankRate'':''0.490202''},{''PointInTime'':1464574500000,''InterbankRate'':''2.040110'',''InverseInterbankRate'':''0.490170''},{''PointInTime'':1464575100000,''InterbankRate'':''2.041089'',''InverseInterbankRate'':''0.489935''},{''PointInTime'':1464575700000,''InterbankRate'':''2.039139'',''InverseInterbankRate'':''0.490403''},{''PointInTime'':1464576300000,''InterbankRate'':''2.039265'',''InverseInterbankRate'':''0.490373''},{''PointInTime'':1464576900000,''InterbankRate'':''2.038953'',''InverseInterbankRate'':''0.490448''},{''PointInTime'':1464577500000,''InterbankRate'':''2.038256'',''InverseInterbankRate'':''0.490615''},{''PointInTime'':1464578100000,''InterbankRate'':''2.039059'',''InverseInterbankRate'':''0.490422''},{''PointInTime'':1464578700000,''InterbankRate'':''2.039105'',''InverseInterbankRate'':''0.490411''},{''PointInTime'':1464579300000,''InterbankRate'':''2.038665'',''InverseInterbankRate'':''0.490517''},{''PointInTime'':1464579900000,''InterbankRate'':''2.038939'',''InverseInterbankRate'':''0.490451''},{''PointInTime'':1464580500000,''InterbankRate'':''2.040179'',''InverseInterbankRate'':''0.490153''},{''PointInTime'':1464581100000,''InterbankRate'':''2.041125'',''InverseInterbankRate'':''0.489926''},{''PointInTime'':1464581700000,''InterbankRate'':''2.041208'',''InverseInterbankRate'':''0.489906''},{''PointInTime'':1464582300000,''InterbankRate'':''2.042680'',''InverseInterbankRate'':''0.489553''},{''PointInTime'':1464582900000,''InterbankRate'':''2.040994'',''InverseInterbankRate'':''0.489957''},{''PointInTime'':1464583500000,''InterbankRate'':''2.042207'',''InverseInterbankRate'':''0.489666''},{''PointInTime'':1464584100000,''InterbankRate'':''2.041685'',''InverseInterbankRate'':''0.489791''},{''PointInTime'':1464584700000,''InterbankRate'':''2.041495'',''InverseInterbankRate'':''0.489837''},{''PointInTime'':1464585300000,''InterbankRate'':''2.040754'',''InverseInterbankRate'':''0.490015''},{''PointInTime'':1464585900000,''InterbankRate'':''2.041093'',''InverseInterbankRate'':''0.489933''},{''PointInTime'':1464586500000,''InterbankRate'':''2.040748'',''InverseInterbankRate'':''0.490016''},{''PointInTime'':1464587100000,''InterbankRate'':''2.040496'',''InverseInterbankRate'':''0.490077''},{''PointInTime'':1464587700000,''InterbankRate'':''2.039964'',''InverseInterbankRate'':''0.490205''},{''PointInTime'':1464588300000,''InterbankRate'':''2.040914'',''InverseInterbankRate'':''0.489977''},{''PointInTime'':1464588900000,''InterbankRate'':''2.040922'',''InverseInterbankRate'':''0.489975''},{''PointInTime'':1464589500000,''InterbankRate'':''2.040768'',''InverseInterbankRate'':''0.490012''},{''PointInTime'':1464590100000,''InterbankRate'':''2.039148'',''InverseInterbankRate'':''0.490401''},{''PointInTime'':1464590700000,''InterbankRate'':''2.038299'',''InverseInterbankRate'':''0.490605''},{''PointInTime'':1464591300000,''InterbankRate'':''2.037275'',''InverseInterbankRate'':''0.490852''},{''PointInTime'':1464591900000,''InterbankRate'':''2.037449'',''InverseInterbankRate'':''0.490810''},{''PointInTime'':1464592500000,''InterbankRate'':''2.037260'',''InverseInterbankRate'':''0.490855''},{''PointInTime'':1464593100000,''InterbankRate'':''2.037545'',''InverseInterbankRate'':''0.490787''},{''PointInTime'':1464593700000,''InterbankRate'':''2.040052'',''InverseInterbankRate'':''0.490184''},{''PointInTime'':1464594300000,''InterbankRate'':''2.038213'',''InverseInterbankRate'':''0.490626''},{''PointInTime'':1464594900000,''InterbankRate'':''2.036616'',''InverseInterbankRate'':''0.491011''},{''PointInTime'':1464595500000,''InterbankRate'':''2.036412'',''InverseInterbankRate'':''0.491060''},{''PointInTime'':1464596100000,''InterbankRate'':''2.036847'',''InverseInterbankRate'':''0.490955''},{''PointInTime'':1464596700000,''InterbankRate'':''2.036869'',''InverseInterbankRate'':''0.490950''},{''PointInTime'':1464597300000,''InterbankRate'':''2.036882'',''InverseInterbankRate'':''0.490946''},{''PointInTime'':1464597900000,''InterbankRate'':''2.035721'',''InverseInterbankRate'':''0.491226''},{''PointInTime'':1464598500000,''InterbankRate'':''2.032501'',''InverseInterbankRate'':''0.492005''},{''PointInTime'':1464599100000,''InterbankRate'':''2.033227'',''InverseInterbankRate'':''0.491829''},{''PointInTime'':1464599700000,''InterbankRate'':''2.032387'',''InverseInterbankRate'':''0.492032''},{''PointInTime'':1464600300000,''InterbankRate'':''2.033671'',''InverseInterbankRate'':''0.491722''},{''PointInTime'':1464600900000,''InterbankRate'':''2.034765'',''InverseInterbankRate'':''0.491457''},{''PointInTime'':1464601500000,''InterbankRate'':''2.033703'',''InverseInterbankRate'':''0.491714''},{''PointInTime'':1464602100000,''InterbankRate'':''2.033475'',''InverseInterbankRate'':''0.491769''},{''PointInTime'':1464602700000,''InterbankRate'':''2.033822'',''InverseInterbankRate'':''0.491685''},{''PointInTime'':1464603300000,''InterbankRate'':''2.034265'',''InverseInterbankRate'':''0.491578''},{''PointInTime'':1464603900000,''InterbankRate'':''2.034148'',''InverseInterbankRate'':''0.491606''},{''PointInTime'':1464604500000,''InterbankRate'':''2.033557'',''InverseInterbankRate'':''0.491749''},{''PointInTime'':1464605100000,''InterbankRate'':''2.033713'',''InverseInterbankRate'':''0.491712''},{''PointInTime'':1464605700000,''InterbankRate'':''2.032677'',''InverseInterbankRate'':''0.491962''},{''PointInTime'':1464606300000,''InterbankRate'':''2.032860'',''InverseInterbankRate'':''0.491918''},{''PointInTime'':1464606900000,''InterbankRate'':''2.032894'',''InverseInterbankRate'':''0.491910''},{''PointInTime'':1464607500000,''InterbankRate'':''2.033186'',''InverseInterbankRate'':''0.491839''},{''PointInTime'':1464608100000,''InterbankRate'':''2.034007'',''InverseInterbankRate'':''0.491640''},{''PointInTime'':1464608700000,''InterbankRate'':''2.034496'',''InverseInterbankRate'':''0.491522''},{''PointInTime'':1464609300000,''InterbankRate'':''2.034304'',''InverseInterbankRate'':''0.491569''},{''PointInTime'':1464609900000,''InterbankRate'':''2.035696'',''InverseInterbankRate'':''0.491232''},{''PointInTime'':1464610500000,''InterbankRate'':''2.036563'',''InverseInterbankRate'':''0.491023''},{''PointInTime'':1464611100000,''InterbankRate'':''2.036395'',''InverseInterbankRate'':''0.491064''},{''PointInTime'':1464611700000,''InterbankRate'':''2.034226'',''InverseInterbankRate'':''0.491587''},{''PointInTime'':1464612300000,''InterbankRate'':''2.034927'',''InverseInterbankRate'':''0.491418''},{''PointInTime'':1464612900000,''InterbankRate'':''2.034764'',''InverseInterbankRate'':''0.491458''},{''PointInTime'':1464613500000,''InterbankRate'':''2.036129'',''InverseInterbankRate'':''0.491128''},{''PointInTime'':1464614100000,''InterbankRate'':''2.036086'',''InverseInterbankRate'':''0.491138''},{''PointInTime'':1464614700000,''InterbankRate'':''2.034837'',''InverseInterbankRate'':''0.491440''},{''PointInTime'':1464615300000,''InterbankRate'':''2.035656'',''InverseInterbankRate'':''0.491242''},{''PointInTime'':1464615900000,''InterbankRate'':''2.035922'',''InverseInterbankRate'':''0.491178''},{''PointInTime'':1464616500000,''InterbankRate'':''2.036330'',''InverseInterbankRate'':''0.491080''},{''PointInTime'':1464617100000,''InterbankRate'':''2.036411'',''InverseInterbankRate'':''0.491060''},{''PointInTime'':1464617700000,''InterbankRate'':''2.035730'',''InverseInterbankRate'':''0.491224''},{''PointInTime'':1464618300000,''InterbankRate'':''2.034149'',''InverseInterbankRate'':''0.491606''},{''PointInTime'':1464618900000,''InterbankRate'':''2.035531'',''InverseInterbankRate'':''0.491272''},{''PointInTime'':1464619500000,''InterbankRate'':''2.035826'',''InverseInterbankRate'':''0.491201''},{''PointInTime'':1464620100000,''InterbankRate'':''2.035537'',''InverseInterbankRate'':''0.491271''},{''PointInTime'':1464620700000,''InterbankRate'':''2.035593'',''InverseInterbankRate'':''0.491257''},{''PointInTime'':1464621300000,''InterbankRate'':''2.036285'',''InverseInterbankRate'':''0.491091''},{''PointInTime'':1464621900000,''InterbankRate'':''2.035059'',''InverseInterbankRate'':''0.491386''},{''PointInTime'':1464622500000,''InterbankRate'':''2.035851'',''InverseInterbankRate'':''0.491195''},{''PointInTime'':1464623100000,''InterbankRate'':''2.036754'',''InverseInterbankRate'':''0.490977''},{''PointInTime'':1464623700000,''InterbankRate'':''2.036855'',''InverseInterbankRate'':''0.490953''},{''PointInTime'':1464624300000,''InterbankRate'':''2.036834'',''InverseInterbankRate'':''0.490958''},{''PointInTime'':1464624900000,''InterbankRate'':''2.037496'',''InverseInterbankRate'':''0.490798''},{''PointInTime'':1464625500000,''InterbankRate'':''2.038082'',''InverseInterbankRate'':''0.490657''},{''PointInTime'':1464626100000,''InterbankRate'':''2.037463'',''InverseInterbankRate'':''0.490807''},{''PointInTime'':1464626700000,''InterbankRate'':''2.037363'',''InverseInterbankRate'':''0.490830''},{''PointInTime'':1464627300000,''InterbankRate'':''2.037206'',''InverseInterbankRate'':''0.490868''},{''PointInTime'':1464627900000,''InterbankRate'':''2.037325'',''InverseInterbankRate'':''0.490840''},{''PointInTime'':1464628500000,''InterbankRate'':''2.036771'',''InverseInterbankRate'':''0.490973''},{''PointInTime'':1464629100000,''InterbankRate'':''2.037480'',''InverseInterbankRate'':''0.490802''},{''PointInTime'':1464629700000,''InterbankRate'':''2.037220'',''InverseInterbankRate'':''0.490865''},{''PointInTime'':1464630300000,''InterbankRate'':''2.036733'',''InverseInterbankRate'':''0.490982''},{''PointInTime'':1464630900000,''InterbankRate'':''2.036533'',''InverseInterbankRate'':''0.491031''},{''PointInTime'':1464631500000,''InterbankRate'':''2.036662'',''InverseInterbankRate'':''0.490999''},{''PointInTime'':1464632100000,''InterbankRate'':''2.036704'',''InverseInterbankRate'':''0.490989''},{''PointInTime'':1464632700000,''InterbankRate'':''2.036617'',''InverseInterbankRate'':''0.491010''},{''PointInTime'':1464633300000,''InterbankRate'':''2.036515'',''InverseInterbankRate'':''0.491035''},{''PointInTime'':1464633900000,''InterbankRate'':''2.036930'',''InverseInterbankRate'':''0.490935''},{''PointInTime'':1464634500000,''InterbankRate'':''2.036636'',''InverseInterbankRate'':''0.491006''},{''PointInTime'':1464635100000,''InterbankRate'':''2.036465'',''InverseInterbankRate'':''0.491047''},{''PointInTime'':1464635700000,''InterbankRate'':''2.036984'',''InverseInterbankRate'':''0.490922''},{''PointInTime'':1464636300000,''InterbankRate'':''2.036981'',''InverseInterbankRate'':''0.490923''},{''PointInTime'':1464636900000,''InterbankRate'':''2.037385'',''InverseInterbankRate'':''0.490825''},{''PointInTime'':1464637500000,''InterbankRate'':''2.037387'',''InverseInterbankRate'':''0.490825''},{''PointInTime'':1464638100000,''InterbankRate'':''2.037166'',''InverseInterbankRate'':''0.490878''},{''PointInTime'':1464638700000,''InterbankRate'':''2.037261'',''InverseInterbankRate'':''0.490855''},{''PointInTime'':1464639300000,''InterbankRate'':''2.037243'',''InverseInterbankRate'':''0.490859''},{''PointInTime'':1464639900000,''InterbankRate'':''2.036857'',''InverseInterbankRate'':''0.490952''},{''PointInTime'':1464640500000,''InterbankRate'':''2.037044'',''InverseInterbankRate'':''0.490908''},{''PointInTime'':1464641100000,''InterbankRate'':''2.037402'',''InverseInterbankRate'':''0.490821''},{''PointInTime'':1464641700000,''InterbankRate'':''2.038429'',''InverseInterbankRate'':''0.490574''},{''PointInTime'':1464642300000,''InterbankRate'':''2.037922'',''InverseInterbankRate'':''0.490696''},{''PointInTime'':1464642900000,''InterbankRate'':''2.037948'',''InverseInterbankRate'':''0.490690''},{''PointInTime'':1464643500000,''InterbankRate'':''2.037614'',''InverseInterbankRate'':''0.490770''},{''PointInTime'':1464644100000,''InterbankRate'':''2.037004'',''InverseInterbankRate'':''0.490917''},{''PointInTime'':1464644700000,''InterbankRate'':''2.036370'',''InverseInterbankRate'':''0.491070''},{''PointInTime'':1464645300000,''InterbankRate'':''2.036505'',''InverseInterbankRate'':''0.491037''},{''PointInTime'':1464645900000,''InterbankRate'':''2.036349'',''InverseInterbankRate'':''0.491075''},{''PointInTime'':1464646500000,''InterbankRate'':''2.036381'',''InverseInterbankRate'':''0.491067''},{''PointInTime'':1464647100000,''InterbankRate'':''2.036397'',''InverseInterbankRate'':''0.491063''},{''PointInTime'':1464647700000,''InterbankRate'':''2.036648'',''InverseInterbankRate'':''0.491003''},{''PointInTime'':1464648300000,''InterbankRate'':''2.036517'',''InverseInterbankRate'':''0.491034''},{''PointInTime'':1464648900000,''InterbankRate'':''2.035031'',''InverseInterbankRate'':''0.491393''},{''PointInTime'':1464649500000,''InterbankRate'':''2.035377'',''InverseInterbankRate'':''0.491309''},{''PointInTime'':1464650100000,''InterbankRate'':''2.034760'',''InverseInterbankRate'':''0.491458''},{''PointInTime'':1464650700000,''InterbankRate'':''2.034439'',''InverseInterbankRate'':''0.491536''},{''PointInTime'':1464651300000,''InterbankRate'':''2.034046'',''InverseInterbankRate'':''0.491631''},{''PointInTime'':1464651900000,''InterbankRate'':''2.034010'',''InverseInterbankRate'':''0.491640''},{''PointInTime'':1464652500000,''InterbankRate'':''2.034014'',''InverseInterbankRate'':''0.491639''},{''PointInTime'':1464653100000,''InterbankRate'':''2.035432'',''InverseInterbankRate'':''0.491296''},{''PointInTime'':1464653700000,''InterbankRate'':''2.036287'',''InverseInterbankRate'':''0.491090''},{''PointInTime'':1464654300000,''InterbankRate'':''2.036800'',''InverseInterbankRate'':''0.490966''},{''PointInTime'':1464654900000,''InterbankRate'':''2.037582'',''InverseInterbankRate'':''0.490778''},{''PointInTime'':1464655500000,''InterbankRate'':''2.037175'',''InverseInterbankRate'':''0.490876''},{''PointInTime'':1464656100000,''InterbankRate'':''2.038306'',''InverseInterbankRate'':''0.490604''}]} from queue: reply:publicsiteofx:mq:c1:930edc9f6ba446e0858775778972f43a','Activity','Prod-OFX-PublicSite.ApiService','OZFAPPSYD02V','265168','170','31.05.2016 11:04:36.240652',,,,");
            }

            var uow1 = LifetimeScope.Resolve<ILowLevelService1>();
            var uow2 = LifetimeScope.Resolve<ILowLevelService2>();
            uow1.Do();
            uow2.Do();

            Console.WriteLine("ID:" + context.Headers.Get("callId", (Guid?) Guid.Empty));
            context.Respond(new CurrencyResponse
            {
                Currencies = new List<CurrencyInfo>
                {
                    new CurrencyInfo {IsoCode = "CAD"},
                    new CurrencyInfo {IsoCode = "AUD"},
                    new CurrencyInfo {IsoCode = "USD"}
                }
            });
            Logger.Information("A call with id {callId} was successfully completed on the worker.");
        }
    }
}