using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureSearchDotNet
{
    class Program
    {
        private static SearchServiceClient cli;
        private static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            configuration = builder.Build();

            cli = CreateSearchServiceClient(configuration);
            
            bool run = true;
            while (run)
            {
                Console.WriteLine("\n\n===\nCreate - Create Index\nLoad- Load Index\nSearch - Search Index\n\nExit - Quit Application\n===");
                Console.Write("Choice: ");
                var choice = Console.ReadLine().ToLower();
                switch (choice)
                {
                    case "create":
                        Create();
                        break;
                    case "load":
                        Load();
                        break;
                    case "search":
                        Search();
                        break;
                    case "exit":
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Choice");
                        break;
                }
            }
        }

        static void Create()
        {
            string indexName = configuration["SearchIndexName"];

            Console.WriteLine("{0}", "Deleting index...\n");
            DeleteIndexIfExists(indexName, cli);

            Console.WriteLine("{0}", "Creating index...\n");
            CreateIndex(indexName, cli);

            Console.WriteLine("{0}", "Create Complete.");
            Console.ReadKey();
        }

        static void Load()
        {
            string json = "[{'First':'Dexter','Last':'James','Phone':'(286) 318-5718','Email':'porttitor.vulputate@risusDonec.net','Birthday':'September 20, 1943'},{'First':'Rama','Last':'Whitley','Phone':'(759) 934-2302','Email':'risus.at.fringilla@primis.org','Birthday':'January 4, 1979'},{'First':'Rowan','Last':'Cohen','Phone':'(420) 452-1428','Email':'ridiculus@euultricessit.edu','Birthday':'February 3, 1976'},{'First':'Lillith','Last':'Mcclain','Phone':'(563) 134-3870','Email':'tellus@liberoettristique.edu','Birthday':'June 29, 1953'},{'First':'Dexter','Last':'George','Phone':'(904) 422-8515','Email':'amet@ametrisusDonec.com','Birthday':'April 3, 1964'},{'First':'Boris','Last':'Becker','Phone':'(227) 378-8018','Email':'aliquet.Proin.velit@porttitortellus.co.uk','Birthday':'February 10, 1979'},{'First':'Judith','Last':'Frazier','Phone':'(348) 330-8419','Email':'non.arcu.Vivamus@Nunc.co.uk','Birthday':'March 28, 1947'},{'First':'Lester','Last':'Barry','Phone':'(517) 588-8279','Email':'et@gravidamaurisut.com','Birthday':'December 9, 1965'},{'First':'Ori','Last':'Greer','Phone':'(582) 688-1338','Email':'urna.Vivamus@mauris.com','Birthday':'October 14, 1982'},{'First':'Kiona','Last':'Spears','Phone':'(243) 983-4769','Email':'Curae.Phasellus@Etiamvestibulum.co.uk','Birthday':'November 1, 1996'},{'First':'James','Last':'Yang','Phone':'(540) 623-2055','Email':'in.felis.Nulla@acmattis.edu','Birthday':'May 6, 1981'},{'First':'Angela','Last':'Farley','Phone':'(766) 849-8990','Email':'sagittis.Nullam.vitae@nisl.ca','Birthday':'December 13, 1990'},{'First':'Martina','Last':'Dean','Phone':'(779) 421-5482','Email':'sollicitudin@Inmi.ca','Birthday':'July 8, 1978'},{'First':'Richard','Last':'Molina','Phone':'(644) 692-1972','Email':'enim.Mauris.quis@molestiedapibus.edu','Birthday':'August 24, 1997'},{'First':'Carolyn','Last':'Maldonado','Phone':'(138) 670-2939','Email':'Sed.auctor.odio@montesnascetur.co.uk','Birthday':'June 6, 1998'},{'First':'Teegan','Last':'Lee','Phone':'(269) 391-6485','Email':'Donec@sitamet.ca','Birthday':'December 7, 1952'},{'First':'Dylan','Last':'Mills','Phone':'(103) 448-6235','Email':'Fusce@semperNamtempor.net','Birthday':'May 6, 1971'},{'First':'Lucy','Last':'Mathews','Phone':'(844) 752-6140','Email':'sem.mollis.dui@tinciduntorciquis.edu','Birthday':'March 24, 1958'},{'First':'Cleo','Last':'Watkins','Phone':'(842) 533-8441','Email':'arcu.Sed.eu@eunullaat.ca','Birthday':'July 21, 1942'},{'First':'Alexa','Last':'Cox','Phone':'(157) 682-5098','Email':'velit@In.com','Birthday':'June 14, 1972'},{'First':'Brandon','Last':'Pate','Phone':'(141) 491-6211','Email':'scelerisque@elitelitfermentum.ca','Birthday':'May 7, 1975'},{'First':'Kiayada','Last':'Good','Phone':'(566) 756-6728','Email':'iaculis.odio.Nam@sollicitudincommodo.net','Birthday':'March 15, 1953'},{'First':'Montana','Last':'Smith','Phone':'(356) 486-2624','Email':'augue.id@enim.edu','Birthday':'July 11, 1941'},{'First':'Tatum','Last':'Neal','Phone':'(890) 166-3260','Email':'sapien@vel.net','Birthday':'January 16, 1998'},{'First':'Ahmed','Last':'Munoz','Phone':'(181) 900-8119','Email':'nec@libero.com','Birthday':'November 13, 1940'},{'First':'Garrett','Last':'Lloyd','Phone':'(508) 547-9703','Email':'Cras.vulputate.velit@elementumsemvitae.com','Birthday':'August 21, 1953'},{'First':'Wayne','Last':'Weber','Phone':'(822) 135-4338','Email':'pede.malesuada@Integerinmagna.org','Birthday':'July 11, 1968'},{'First':'Valentine','Last':'Raymond','Phone':'(810) 407-7351','Email':'Praesent.interdum.ligula@Phasellusfermentumconvallis.ca','Birthday':'July 26, 1947'},{'First':'Kenyon','Last':'Richard','Phone':'(973) 487-7536','Email':'molestie.orci@sapienAenean.ca','Birthday':'April 27, 1938'},{'First':'Evan','Last':'Price','Phone':'(832) 926-8281','Email':'scelerisque@turpisnonenim.ca','Birthday':'November 19, 2000'},{'First':'Rashad','Last':'Marquez','Phone':'(248) 835-4872','Email':'vel@Integer.ca','Birthday':'November 12, 1951'},{'First':'Kerry','Last':'Beard','Phone':'(909) 235-8478','Email':'enim.Etiam.imperdiet@mattisvelitjusto.com','Birthday':'July 28, 1963'},{'First':'Melvin','Last':'Lang','Phone':'(911) 673-6042','Email':'turpis.Aliquam@dapibus.co.uk','Birthday':'July 16, 1955'},{'First':'Eve','Last':'Garner','Phone':'(796) 396-7499','Email':'ligula.Aliquam.erat@commodotincidunt.org','Birthday':'August 26, 1988'},{'First':'Winifred','Last':'Pittman','Phone':'(336) 616-3772','Email':'nunc.sed@dolornonummy.co.uk','Birthday':'December 19, 1962'},{'First':'Mariko','Last':'Kline','Phone':'(304) 504-9705','Email':'ante.blandit@ut.net','Birthday':'April 28, 1967'},{'First':'Silas','Last':'French','Phone':'(453) 651-2634','Email':'massa@a.org','Birthday':'December 9, 1999'},{'First':'Keefe','Last':'Velasquez','Phone':'(805) 707-6505','Email':'Nullam.lobortis.quam@nibh.com','Birthday':'December 7, 1938'},{'First':'Susan','Last':'Black','Phone':'(778) 751-5761','Email':'sagittis.semper@Namac.co.uk','Birthday':'May 10, 1942'},{'First':'Wayne','Last':'Thomas','Phone':'(677) 417-4348','Email':'metus@tellus.org','Birthday':'February 18, 1937'},{'First':'Aileen','Last':'Miles','Phone':'(510) 646-3527','Email':'orci@dictum.com','Birthday':'January 21, 1975'},{'First':'Keelie','Last':'Wood','Phone':'(645) 201-1974','Email':'ridiculus.mus@Aliquam.edu','Birthday':'January 12, 1998'},{'First':'Peter','Last':'Reid','Phone':'(873) 681-6473','Email':'arcu@ante.org','Birthday':'June 11, 1968'},{'First':'Lawrence','Last':'Henson','Phone':'(142) 338-8916','Email':'dis.parturient.montes@nonenim.net','Birthday':'December 15, 1978'},{'First':'Brynne','Last':'Daniel','Phone':'(786) 924-3010','Email':'ac@Vivamusnisi.ca','Birthday':'September 1, 1985'},{'First':'Jelani','Last':'Prince','Phone':'(839) 710-3601','Email':'non.dapibus@maurisIntegersem.com','Birthday':'June 10, 1982'},{'First':'Scarlett','Last':'Cervantes','Phone':'(285) 701-6590','Email':'et@magnaet.net','Birthday':'November 6, 1961'},{'First':'Jennifer','Last':'Tucker','Phone':'(727) 690-9144','Email':'magna@mattisIntegereu.edu','Birthday':'July 13, 1956'},{'First':'Matthew','Last':'Koch','Phone':'(756) 664-9143','Email':'in.tempus.eu@aodio.co.uk','Birthday':'January 20, 1984'},{'First':'Linda','Last':'Parsons','Phone':'(498) 885-8786','Email':'dictum.magna@orciadipiscing.com','Birthday':'October 30, 2001'},{'First':'Gavin','Last':'Reed','Phone':'(931) 917-2211','Email':'convallis.ante.lectus@Aliquamultricesiaculis.net','Birthday':'August 30, 1986'},{'First':'Lani','Last':'Williamson','Phone':'(670) 935-6874','Email':'Maecenas.malesuada.fringilla@Vestibulumanteipsum.edu','Birthday':'August 26, 1943'},{'First':'Brynne','Last':'Shaffer','Phone':'(475) 875-8821','Email':'turpis.vitae@semNullainterdum.net','Birthday':'January 15, 1935'},{'First':'Sheila','Last':'Sims','Phone':'(734) 350-0545','Email':'et.eros@dolorvitaedolor.org','Birthday':'July 14, 1946'},{'First':'Candice','Last':'Trevino','Phone':'(951) 377-7790','Email':'est.tempor@dui.edu','Birthday':'August 12, 1990'},{'First':'Burton','Last':'Pearson','Phone':'(824) 947-5363','Email':'sem.eget@amet.edu','Birthday':'November 26, 1966'},{'First':'Liberty','Last':'Campos','Phone':'(254) 549-0098','Email':'vulputate.mauris@estac.net','Birthday':'March 23, 1968'},{'First':'Fleur','Last':'Bender','Phone':'(959) 751-9135','Email':'Donec.tempor.est@Loremipsum.org','Birthday':'March 8, 1952'},{'First':'Quentin','Last':'Wolfe','Phone':'(834) 879-7881','Email':'magnis.dis.parturient@turpisNulla.ca','Birthday':'April 15, 1991'},{'First':'Thaddeus','Last':'Rhodes','Phone':'(600) 358-4556','Email':'elit.fermentum@tincidunt.com','Birthday':'October 7, 1944'},{'First':'Blake','Last':'Copeland','Phone':'(639) 258-2841','Email':'dictum.sapien.Aenean@ridiculusmusProin.ca','Birthday':'September 3, 1954'},{'First':'Drew','Last':'Olsen','Phone':'(242) 801-3335','Email':'vitae.odio@Integersemelit.org','Birthday':'July 15, 1941'},{'First':'Dieter','Last':'Delaney','Phone':'(924) 837-6017','Email':'ut.erat@in.com','Birthday':'July 19, 1958'},{'First':'Rhiannon','Last':'Glass','Phone':'(229) 794-8867','Email':'pretium.neque@tortordictum.co.uk','Birthday':'November 30, 1996'},{'First':'Tasha','Last':'Mcclain','Phone':'(978) 745-3756','Email':'ridiculus.mus@velitegetlaoreet.net','Birthday':'August 13, 1982'},{'First':'Yardley','Last':'Vasquez','Phone':'(698) 146-6214','Email':'lacus.Quisque.purus@Donecelementumlorem.edu','Birthday':'November 5, 1980'},{'First':'Cecilia','Last':'Best','Phone':'(583) 607-5396','Email':'nunc@Integerid.org','Birthday':'January 3, 1967'},{'First':'Taylor','Last':'Fulton','Phone':'(958) 232-2257','Email':'metus.eu@duinec.ca','Birthday':'March 19, 1972'},{'First':'Alma','Last':'Mccall','Phone':'(923) 249-5458','Email':'sed.est@consequatauctor.org','Birthday':'August 22, 1986'},{'First':'Nash','Last':'Mayo','Phone':'(347) 322-2693','Email':'mi.pede.nonummy@estmollis.co.uk','Birthday':'January 4, 1935'},{'First':'Elton','Last':'Henderson','Phone':'(103) 289-2262','Email':'enim.Mauris.quis@elit.co.uk','Birthday':'December 28, 1983'},{'First':'Timon','Last':'Velasquez','Phone':'(863) 754-6388','Email':'ligula.Nullam.enim@urna.org','Birthday':'December 14, 1965'},{'First':'Lois','Last':'Combs','Phone':'(932) 654-1483','Email':'aliquet@blanditmattis.co.uk','Birthday':'June 11, 1975'},{'First':'Delilah','Last':'Trevino','Phone':'(629) 235-0193','Email':'iaculis@Fuscemollis.edu','Birthday':'June 1, 1941'},{'First':'Nicole','Last':'Andrews','Phone':'(953) 224-8807','Email':'elementum.lorem@massa.org','Birthday':'April 25, 1960'},{'First':'Galena','Last':'Hernandez','Phone':'(197) 289-5224','Email':'neque.Morbi.quis@ligulaelitpretium.ca','Birthday':'November 7, 1958'},{'First':'Laith','Last':'Miles','Phone':'(520) 919-7073','Email':'malesuada@malesuadafamesac.ca','Birthday':'June 15, 1944'},{'First':'Aaron','Last':'Mckenzie','Phone':'(549) 615-7599','Email':'est.Nunc.ullamcorper@Namconsequatdolor.org','Birthday':'August 29, 1983'},{'First':'Ayanna','Last':'Andrews','Phone':'(595) 725-8085','Email':'at.iaculis@Nullamsuscipit.edu','Birthday':'September 20, 1991'},{'First':'Dominique','Last':'Lambert','Phone':'(854) 762-7857','Email':'enim@Aliquamadipiscing.com','Birthday':'May 20, 1976'},{'First':'Ruth','Last':'Hobbs','Phone':'(915) 431-7337','Email':'Nunc.pulvinar@primisinfaucibus.net','Birthday':'March 28, 1971'},{'First':'Ava','Last':'Charles','Phone':'(922) 316-7703','Email':'semper@nonfeugiat.org','Birthday':'December 20, 1996'},{'First':'Galvin','Last':'Wiggins','Phone':'(711) 447-2922','Email':'Phasellus.at.augue@scelerisque.org','Birthday':'October 26, 1965'},{'First':'Lani','Last':'Dyer','Phone':'(937) 110-0311','Email':'sem.magna.nec@acmattis.org','Birthday':'March 17, 1958'},{'First':'Fallon','Last':'Terry','Phone':'(725) 566-6131','Email':'ut.odio.vel@tempusmauris.edu','Birthday':'August 7, 1948'},{'First':'Phelan','Last':'Donaldson','Phone':'(326) 763-1939','Email':'gravida.nunc@sociis.co.uk','Birthday':'October 31, 1981'},{'First':'Zane','Last':'Acevedo','Phone':'(692) 983-3300','Email':'vitae.odio.sagittis@nisi.net','Birthday':'March 12, 1947'},{'First':'Duncan','Last':'Dorsey','Phone':'(318) 936-4345','Email':'sed@Praesentluctus.co.uk','Birthday':'June 23, 1961'},{'First':'Roanna','Last':'Kramer','Phone':'(129) 315-5639','Email':'Donec.egestas@Quisquetincidunt.net','Birthday':'July 3, 1943'},{'First':'Hyatt','Last':'Walsh','Phone':'(819) 904-5904','Email':'nunc.ac.mattis@elit.com','Birthday':'August 28, 1992'},{'First':'Roary','Last':'Bowers','Phone':'(515) 164-3315','Email':'mauris@orcitincidunt.ca','Birthday':'August 22, 1973'},{'First':'Drew','Last':'Hernandez','Phone':'(676) 385-7342','Email':'vitae@acfermentumvel.net','Birthday':'March 19, 1998'},{'First':'Angela','Last':'Conrad','Phone':'(884) 166-2116','Email':'mollis@cursusluctusipsum.net','Birthday':'August 5, 1942'},{'First':'Pamela','Last':'Pratt','Phone':'(457) 597-9051','Email':'Phasellus@necenimNunc.co.uk','Birthday':'February 9, 1957'},{'First':'Arthur','Last':'Larson','Phone':'(873) 461-2449','Email':'tempus.scelerisque.lorem@sit.ca','Birthday':'July 12, 1974'},{'First':'Remedios','Last':'Nichols','Phone':'(459) 905-7255','Email':'metus.urna@et.org','Birthday':'May 25, 1981'},{'First':'Nehru','Last':'Reid','Phone':'(438) 334-5265','Email':'commodo.ipsum.Suspendisse@metusfacilisis.ca','Birthday':'September 18, 1955'},{'First':'Rhiannon','Last':'Roberts','Phone':'(476) 232-8918','Email':'arcu@lectusasollicitudin.co.uk','Birthday':'January 12, 1934'},{'First':'Keegan','Last':'Baker','Phone':'(724) 887-5164','Email':'tempor@augueutlacus.ca','Birthday':'September 22, 1974'},{'First':'Carol','Last':'Deleon','Phone':'(194) 747-8300','Email':'In.mi.pede@lacinia.com','Birthday':'October 10, 1958'},{'First':'Regina','Last':'Davis','Phone':'(555) 817-7585','Email':'arcu@acturpis.net','Birthday':'September 21, 1938'},{'First':'Shana','Last':'Sosa','Phone':'(676) 904-8531','Email':'nibh.sit@tellus.ca','Birthday':'January 4, 1979'},{'First':'Rogan','Last':'Floyd','Phone':'(152) 322-9601','Email':'convallis.convallis@velitduisemper.org','Birthday':'January 29, 1956'},{'First':'Vaughan','Last':'Larsen','Phone':'(601) 994-0893','Email':'ut.quam@Integervulputaterisus.ca','Birthday':'December 21, 1958'},{'First':'Anjolie','Last':'Webb','Phone':'(433) 551-6936','Email':'orci.Donec@eumetusIn.co.uk','Birthday':'March 16, 1946'},{'First':'Faith','Last':'Kerr','Phone':'(603) 795-6853','Email':'euismod.enim@loremvehicula.org','Birthday':'November 3, 1965'},{'First':'Andrew','Last':'Holt','Phone':'(967) 712-7020','Email':'Duis@accumsan.com','Birthday':'August 25, 1981'},{'First':'Sade','Last':'Mcknight','Phone':'(848) 394-3959','Email':'sit@mipede.edu','Birthday':'March 14, 1971'},{'First':'Vladimir','Last':'Ingram','Phone':'(256) 340-0738','Email':'risus@velmauris.net','Birthday':'September 27, 1966'},{'First':'Rebekah','Last':'Lancaster','Phone':'(461) 966-6992','Email':'mauris@infaucibus.org','Birthday':'June 14, 1943'},{'First':'Judah','Last':'Branch','Phone':'(802) 690-8961','Email':'nibh.Phasellus@lorem.com','Birthday':'January 17, 1953'},{'First':'Simone','Last':'Duran','Phone':'(891) 967-4334','Email':'elit.a@non.com','Birthday':'May 18, 1968'},{'First':'Renee','Last':'Hurley','Phone':'(902) 201-1972','Email':'turpis.In@Cras.net','Birthday':'October 14, 1934'},{'First':'Carson','Last':'Norton','Phone':'(730) 990-3148','Email':'lobortis.risus@Cras.edu','Birthday':'December 29, 1954'},{'First':'Ulla','Last':'Boyle','Phone':'(812) 277-4590','Email':'Sed.diam.lorem@eliteratvitae.co.uk','Birthday':'March 7, 1968'},{'First':'Baker','Last':'Harrington','Phone':'(150) 814-1428','Email':'gravida.molestie@mauriselitdictum.com','Birthday':'July 14, 1948'},{'First':'Jacob','Last':'Hamilton','Phone':'(129) 292-2372','Email':'pede.Suspendisse.dui@Duisacarcu.ca','Birthday':'April 19, 1998'},{'First':'Charles','Last':'Hawkins','Phone':'(745) 607-7195','Email':'luctus.aliquet@adipiscingnonluctus.com','Birthday':'June 10, 1969'},{'First':'Avram','Last':'Riggs','Phone':'(769) 345-9828','Email':'suscipit@urnanecluctus.ca','Birthday':'July 18, 1944'},{'First':'Virginia','Last':'Foreman','Phone':'(180) 961-2505','Email':'nec.ante@Suspendissecommodotincidunt.com','Birthday':'September 21, 1987'},{'First':'Caryn','Last':'Austin','Phone':'(672) 458-3519','Email':'nonummy.ut.molestie@arcuVestibulumut.org','Birthday':'August 6, 1996'},{'First':'Leroy','Last':'Miranda','Phone':'(491) 494-5366','Email':'Praesent@elit.edu','Birthday':'June 21, 1945'},{'First':'Melyssa','Last':'Macdonald','Phone':'(510) 162-7315','Email':'tellus@sem.edu','Birthday':'February 28, 1978'},{'First':'Quemby','Last':'Combs','Phone':'(207) 351-8617','Email':'faucibus@convallisconvallis.ca','Birthday':'May 8, 1946'},{'First':'Mara','Last':'Warner','Phone':'(883) 284-8988','Email':'facilisi@rutrum.com','Birthday':'September 11, 1991'},{'First':'Lamar','Last':'Patel','Phone':'(314) 573-8867','Email':'Integer@Nunclaoreetlectus.ca','Birthday':'July 1, 1953'},{'First':'Charde','Last':'Cohen','Phone':'(874) 292-2648','Email':'convallis@Fuscemollis.edu','Birthday':'July 16, 1996'},{'First':'Wylie','Last':'Gross','Phone':'(550) 898-7417','Email':'luctus@a.ca','Birthday':'November 19, 1957'},{'First':'Ella','Last':'Ware','Phone':'(904) 669-8972','Email':'porttitor.tellus.non@Proinultrices.com','Birthday':'July 17, 1971'},{'First':'Igor','Last':'Olson','Phone':'(900) 313-1423','Email':'Cras@arcuimperdiet.co.uk','Birthday':'March 18, 1958'},{'First':'Cynthia','Last':'Hatfield','Phone':'(812) 378-5924','Email':'aliquet.nec.imperdiet@fringilla.org','Birthday':'December 10, 1970'},{'First':'Madaline','Last':'Dale','Phone':'(270) 305-2937','Email':'Ut.nec@leoin.co.uk','Birthday':'April 18, 1974'},{'First':'Luke','Last':'Everett','Phone':'(589) 213-3312','Email':'Vivamus.rhoncus@est.edu','Birthday':'October 25, 1955'},{'First':'Odessa','Last':'Mcclain','Phone':'(117) 543-0842','Email':'Lorem.ipsum@quis.org','Birthday':'July 2, 1968'},{'First':'Karen','Last':'Christensen','Phone':'(208) 622-8219','Email':'orci@Nullamsuscipit.com','Birthday':'June 7, 1991'},{'First':'Dexter','Last':'Workman','Phone':'(176) 162-9976','Email':'arcu@consequatdolor.ca','Birthday':'June 17, 1977'},{'First':'Ethan','Last':'Randolph','Phone':'(471) 474-0704','Email':'adipiscing.elit@Donec.net','Birthday':'July 8, 1944'},{'First':'Noel','Last':'Foster','Phone':'(484) 778-0988','Email':'a@ligula.ca','Birthday':'October 17, 1972'},{'First':'Shea','Last':'Owen','Phone':'(351) 587-8358','Email':'Mauris.nulla@liberoest.net','Birthday':'May 5, 1952'},{'First':'Lucy','Last':'Freeman','Phone':'(589) 326-1431','Email':'mollis.Phasellus.libero@a.com','Birthday':'June 19, 1978'},{'First':'Jamalia','Last':'Hooper','Phone':'(920) 873-6141','Email':'Vivamus.sit.amet@temporest.net','Birthday':'February 19, 1938'},{'First':'Leonard','Last':'Mccray','Phone':'(798) 410-0778','Email':'urna.Nunc@ullamcorpernislarcu.net','Birthday':'March 30, 1945'},{'First':'Oprah','Last':'Bentley','Phone':'(846) 954-1383','Email':'dolor.Quisque@magnaCrasconvallis.ca','Birthday':'June 15, 1967'},{'First':'Warren','Last':'Barber','Phone':'(538) 568-5817','Email':'tellus@Donecluctusaliquet.edu','Birthday':'February 18, 1943'},{'First':'Camille','Last':'Grant','Phone':'(741) 318-1276','Email':'consequat@ultricesiaculis.ca','Birthday':'January 2, 1945'},{'First':'Gil','Last':'Quinn','Phone':'(591) 652-9192','Email':'erat@condimentumDonecat.edu','Birthday':'March 23, 1999'},{'First':'Stone','Last':'Haley','Phone':'(233) 998-9882','Email':'tellus.faucibus@hendrerita.co.uk','Birthday':'October 6, 1996'},{'First':'Gemma','Last':'Strong','Phone':'(445) 879-9793','Email':'aliquet.libero@consectetueradipiscingelit.net','Birthday':'November 10, 1937'},{'First':'Colby','Last':'Guzman','Phone':'(205) 371-8386','Email':'erat@vulputate.ca','Birthday':'March 21, 1984'},{'First':'Cullen','Last':'Allison','Phone':'(110) 159-7003','Email':'elit.Etiam@rutrumloremac.ca','Birthday':'December 1, 1955'},{'First':'Chantale','Last':'Moss','Phone':'(841) 927-9011','Email':'mauris.blandit@egestashendreritneque.com','Birthday':'July 13, 1935'},{'First':'Casey','Last':'Buck','Phone':'(709) 270-0643','Email':'eu@utipsum.edu','Birthday':'February 23, 1937'},{'First':'Amos','Last':'Love','Phone':'(749) 881-6274','Email':'vehicula.Pellentesque.tincidunt@egetlacusMauris.com','Birthday':'June 1, 1980'},{'First':'Urielle','Last':'Fitzgerald','Phone':'(647) 154-5229','Email':'vehicula.risus@risus.com','Birthday':'February 27, 1968'},{'First':'Emery','Last':'Galloway','Phone':'(146) 408-5840','Email':'Pellentesque.habitant@ullamcorperviverra.org','Birthday':'May 2, 1965'},{'First':'Mufutau','Last':'Boone','Phone':'(472) 206-0161','Email':'lorem@FuscemollisDuis.org','Birthday':'May 22, 1987'},{'First':'Stephen','Last':'Benjamin','Phone':'(858) 350-2705','Email':'tempus.lorem@diam.ca','Birthday':'October 7, 1984'},{'First':'Rudyard','Last':'Waters','Phone':'(229) 439-4032','Email':'viverra.Donec.tempus@aduiCras.co.uk','Birthday':'March 10, 1999'},{'First':'Malachi','Last':'Cain','Phone':'(919) 287-9867','Email':'ipsum.dolor.sit@magnaa.edu','Birthday':'July 23, 1990'},{'First':'Trevor','Last':'Clements','Phone':'(156) 811-0109','Email':'pretium.aliquet@nonsapien.org','Birthday':'February 11, 1974'},{'First':'Charlotte','Last':'Simon','Phone':'(930) 714-0487','Email':'viverra.Maecenas@orcitincidunt.ca','Birthday':'July 2, 1996'},{'First':'Orlando','Last':'Lester','Phone':'(401) 100-7676','Email':'lectus.justo@cursuspurusNullam.edu','Birthday':'August 24, 1989'},{'First':'Gabriel','Last':'Mercer','Phone':'(578) 243-1092','Email':'quis@quam.net','Birthday':'December 4, 1950'},{'First':'Medge','Last':'Riggs','Phone':'(817) 548-9481','Email':'vestibulum.Mauris@Mauris.com','Birthday':'May 30, 1936'},{'First':'Derek','Last':'Holland','Phone':'(837) 203-1688','Email':'mauris.sapien@dolorQuisquetincidunt.net','Birthday':'December 4, 1997'},{'First':'Ira','Last':'Delgado','Phone':'(576) 758-1760','Email':'sed.leo@luctus.ca','Birthday':'July 24, 1934'},{'First':'Nora','Last':'Frost','Phone':'(827) 899-2295','Email':'augue.porttitor@luctuset.com','Birthday':'September 3, 1934'},{'First':'Ursula','Last':'White','Phone':'(369) 321-1494','Email':'ac.risus@loremvitae.ca','Birthday':'December 5, 1965'},{'First':'Igor','Last':'Guerrero','Phone':'(352) 581-5552','Email':'metus@sedhendrerita.ca','Birthday':'April 17, 1976'},{'First':'Clarke','Last':'Hawkins','Phone':'(303) 501-1564','Email':'libero@ornareegestas.co.uk','Birthday':'October 12, 1992'},{'First':'Louis','Last':'Carson','Phone':'(949) 891-4330','Email':'elit.Curabitur.sed@rutrummagnaCras.co.uk','Birthday':'June 22, 1965'},{'First':'Arden','Last':'Juarez','Phone':'(290) 259-9417','Email':'dignissim.Maecenas@commodo.edu','Birthday':'December 22, 1952'},{'First':'Larissa','Last':'Serrano','Phone':'(951) 900-9609','Email':'enim@venenatisamagna.net','Birthday':'October 21, 1983'},{'First':'Martina','Last':'Cortez','Phone':'(348) 318-8047','Email':'elit.pede@quis.edu','Birthday':'June 14, 1934'},{'First':'Tatum','Last':'Robles','Phone':'(112) 228-4216','Email':'purus.Maecenas@Utsagittislobortis.net','Birthday':'December 7, 1993'},{'First':'Kareem','Last':'Mcmahon','Phone':'(423) 292-6786','Email':'enim.sit.amet@ami.co.uk','Birthday':'August 25, 1955'},{'First':'Bryar','Last':'Blair','Phone':'(728) 430-5335','Email':'diam@anequeNullam.edu','Birthday':'January 27, 1978'},{'First':'Emmanuel','Last':'Juarez','Phone':'(528) 650-6754','Email':'turpis.nec.mauris@Fusce.com','Birthday':'July 23, 1937'},{'First':'Lester','Last':'Salazar','Phone':'(642) 225-4295','Email':'sodales.nisi@vitae.edu','Birthday':'November 29, 1954'},{'First':'Brenna','Last':'Maynard','Phone':'(939) 934-2258','Email':'dapibus.rutrum.justo@temporbibendum.ca','Birthday':'January 9, 1994'},{'First':'Natalie','Last':'Robertson','Phone':'(400) 841-6764','Email':'vel@Aliquamtinciduntnunc.edu','Birthday':'April 9, 1966'},{'First':'Chanda','Last':'Chan','Phone':'(589) 867-3601','Email':'velit@elitfermentumrisus.com','Birthday':'December 14, 1991'},{'First':'Kim','Last':'Ross','Phone':'(177) 747-1360','Email':'Sed.molestie@sapien.ca','Birthday':'March 24, 1980'},{'First':'Thaddeus','Last':'Rosa','Phone':'(767) 178-9159','Email':'lacus.Mauris@Ut.ca','Birthday':'January 5, 1940'},{'First':'Lynn','Last':'Warner','Phone':'(955) 836-1174','Email':'est@aodio.ca','Birthday':'February 18, 1972'},{'First':'Gareth','Last':'Becker','Phone':'(285) 919-7132','Email':'semper@sit.co.uk','Birthday':'December 29, 1982'},{'First':'Rana','Last':'Shaw','Phone':'(949) 967-4751','Email':'tincidunt.nibh@eueleifendnec.com','Birthday':'October 5, 1956'},{'First':'Teagan','Last':'Stout','Phone':'(441) 507-0799','Email':'senectus.et.netus@lectusa.com','Birthday':'September 19, 1971'},{'First':'Gareth','Last':'Combs','Phone':'(130) 913-0786','Email':'risus.Donec.egestas@cursus.edu','Birthday':'January 18, 1949'},{'First':'Aline','Last':'Barron','Phone':'(469) 646-3060','Email':'senectus.et.netus@facilisismagna.org','Birthday':'June 7, 1941'},{'First':'Selma','Last':'Perry','Phone':'(819) 103-9013','Email':'enim.Nunc@non.net','Birthday':'July 14, 1991'},{'First':'Ivan','Last':'Mayo','Phone':'(641) 880-9723','Email':'quis.arcu@etpede.com','Birthday':'February 4, 1996'},{'First':'Chaney','Last':'Mcguire','Phone':'(797) 308-3589','Email':'tempor.bibendum@neque.org','Birthday':'December 20, 1992'},{'First':'Justina','Last':'Anderson','Phone':'(728) 164-2656','Email':'tincidunt@ullamcorperviverra.org','Birthday':'May 24, 1980'},{'First':'Karleigh','Last':'Larson','Phone':'(176) 942-8696','Email':'Quisque.libero.lacus@elitpellentesque.edu','Birthday':'July 10, 1966'},{'First':'Zelda','Last':'Berg','Phone':'(167) 553-3839','Email':'metus.Aenean.sed@per.net','Birthday':'March 7, 1942'},{'First':'Ulric','Last':'Roberts','Phone':'(480) 475-6430','Email':'sed@semmollisdui.com','Birthday':'November 24, 1949'},{'First':'Bertha','Last':'Fisher','Phone':'(995) 423-9688','Email':'Duis.at.lacus@sit.edu','Birthday':'April 8, 1954'},{'First':'Justin','Last':'Reeves','Phone':'(133) 771-9660','Email':'turpis.Aliquam.adipiscing@infelisNulla.co.uk','Birthday':'July 14, 1942'},{'First':'Bevis','Last':'Haney','Phone':'(809) 759-1673','Email':'posuere@accumsanlaoreetipsum.org','Birthday':'March 20, 1957'}]";

            List<Person> people = JsonConvert.DeserializeObject<List<Person>>(json, new IsoDateTimeConverter { DateTimeFormat = "MMMM d, yyyy" });

            people.ForEach(p => p.Id = Guid.NewGuid().ToString());

            var actions = new IndexAction<Person>[people.Count];
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i] = IndexAction.Upload(people[i]);

            }

            var batch = IndexBatch.New(actions);

            string indexName = configuration["SearchIndexName"];

            var indexClient = cli.Indexes.GetClient(indexName);

            try
            {
                indexClient.Documents.Index(batch);
            }
            catch (IndexBatchException e)
            {
                Console.WriteLine(
                    "Failed to index some of the documents: {0}",
                    String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }
        }

        static void Search()
        {
            SearchParameters parameters;
            DocumentSearchResult<Person> results;

            string indexName = configuration["SearchIndexName"];

            var indexClient = cli.Indexes.GetClient(indexName);

            Console.Write("Search Term: ");
            var term = Console.ReadLine();

            parameters = new SearchParameters();
            results = indexClient.Documents.Search<Person>(term, parameters);
            WriteDocuments(results);
        }

        #region Helpers

        private static SearchServiceClient CreateSearchServiceClient(IConfigurationRoot configuration)
        {
            string searchServiceName = configuration["SearchServiceName"];
            string adminApiKey = configuration["SearchServiceAdminApiKey"];

            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            return serviceClient;
        }

        private static void DeleteIndexIfExists(string indexName, SearchServiceClient serviceClient)
        {
            if (serviceClient.Indexes.Exists(indexName))
            {
                serviceClient.Indexes.Delete(indexName);
            }
        }

        private static void CreateIndex(string indexName, SearchServiceClient serviceClient)
        {
            var definition = new Microsoft.Azure.Search.Models.Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<Person>()
            };

            serviceClient.Indexes.Create(definition);
        }

        private static void WriteDocuments(DocumentSearchResult<Person> searchResults)
        {
            foreach (SearchResult<Person> result in searchResults.Results)
            {
                Console.WriteLine("{0} {1}", result.Document, result.Score);
            }

            Console.WriteLine();
        }

        #endregion
    }

    public class Person
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string Id { get; set; }

        [IsSearchable, IsSortable]
        public string First { get; set; }

        [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        public string Last { get; set; }

        [IsSearchable]
        public string Phone { get; set; }

        [IsSearchable, IsSortable]
        public string Email { get; set; }

        [IsFilterable, IsSortable, IsFacetable]
        public DateTime Birthday { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
