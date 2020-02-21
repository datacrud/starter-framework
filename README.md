## Starter Framework
A boilerplate to getting started a project concerning the business logic instead of core logics like User Management, Role Management, Permission Management, Tenant Management, Subscription Management etc. 
The core technologies we are using Asp.Net Web API, Angularjs, Asp.Net Identity, Unity DI, Hangfire, Bootstrap, Angular UI Bootstrap, Angular UI Router, Gulp etc.


### Quick Start
1. First of all clone the repository
2. Then open solution in visual studio
3. This is a client and server distributed solution. The client starting point is the '01 Client/ Client' project and the server starting point is the '02 Server/ Server' project. 
4. Update your connection string in Server.csproj web.config
5. Run the server project using CTRL + F5 (Make sure the Server is set as starter project)
6. On client project the starting point is index.dev.html. You can run it using CTRL + Shift + W (Make sure the index.dev.html file is selected first). The client project has some bower dependencies. Before running kindly ensure that you run the successful bower install command from your console. To use the gulp versioning distribution you will need to install the node dependencies. But for development, it is not required.
7. You will find the login screen. Use the following credentials to log in. 
           
           Company ID: host           
           Username: systemadmin
           Password: 123qwe
           You can check the credentials on SecurityModelSeedDataManager.cs file
           
8. Now the project has been set and run. You can start adding your model and complete the CRUD operation within a very short time. You can see the sample code of Customer, Supplier, Warehouse etc.
  
 

### My First Model CRUD
Suppose I need a CRUD operation for a global note, where the Note model has two properties one is Comment and 2ed one in Reference. So let's create the CRUD now.

1. At first, add a Note.cs model in Model.csproj
     
     
           public class Note : HaveTenantIdEntityBase
           {
               public string Comment {get;set;}
               public string Reference {get;set;}
            }
     
     
          
2. In the 2ed step create a view model NoteViewModel.cs in ViewModel.csproj
 
       
       
           public class NoteViewModel : HaveTenantIdViewModelBase<Note>
           {
               public string Comment {get;set;}
               public string Reference {get;set;}
               
                public NoteViewModel(Note model): base(model)
                {
                    Comment = model.Comment;
                    Reference = model.Reference;
                }
        
            }
            
            
            
3. In 3ed step create a request model named NoteRequestModel.cs in RequestModel.csproj



           public class NoteRequestModel : HaveTenantIdRequestModelBase<Note>
          {
                public NoteRequestModel(string keyword, string orderBy, string isAscending) : base(keyword, orderBy, isAscending)
                {
                }

                public override Expression<Func<Note, bool>> GetExpression()
                {
                    if (!string.IsNullOrWhiteSpace(Keyword))
                    {
                        ExpressionObj = x => x.Comment.Contains(Keyword);
                    }

                }
                
                return ExpressionObj;
          }
          
          

4. In 4th step create a manager named NoteRepository.cs with an interface in Repository.csproj



           public interface INoteRepository : IHaveTenantIdRepositoryBase<Note>
          {
          }

          public class NoteRepository :HaveTenantIdRepositoryBase<Note>, INoteRepository
          {      
                public NoteRepository(BusinessDbContext db) : base(db)
                {

                }       
          }
          
          
          
5. Next step create a note service named NoteService.cs with an interface in Service.csproj



           public interface INoteService : HaveTenantIdServiceBase<Note>
          {
          }

          public class NoteService :HaveTenantIdServiceBase<Note>, INoteService
          {      
                private readonly INoteRepository _repository;
              
                public NoteRepository(INoteRepository repository) : base(repository)
                {
                    _repository = repository;
                }       
          }




6. This is the Server last step, create the API controller for note NoteController.cs in Server.csproj



          public class NoteController : HaveTenantIdControllerBase<Note, NoteViewModel, NoteRequestModel>
          {
                private readonly INoteService _service;                

                public CompanyController(INoteService service) : base(service)
                {
                    _service = service;
                }
          }
          
          
          
7. Build the Server project. Note API is ready now. You can create, update, delete, get by id and paging get all through the API without writing any line of code for those. The API can be accessed through {{serverbasepath}}/api/Note


8. Let's create the client now. First of all, find the url.service.js  and add the API reference in this file to control it globally.
    
          
          self.urls.NoteUrls = noteUrls(self.urls.baseApi);
          
          var noteUrls = function (baseApiUrl) {
              var urls = [];
              urls.Note = baseApiUrl + "Note";

              return urls;
          };
       
       
9. Create an angularjs controller in Client website following path Client/app/scripts/notes/note.controller.js and add the js reference in index.dev.html


          angular.module("app")
            .controller("NoteController", [
                "$scope", "$rootScope", "UrlService", "$controller", "ServiceBase", "$stateParams",
                function ($scope, $rootScope, urlService, $controller, baseService, $stateParams) {
                  "use strict";

                  var config = function() {
                      baseService.setCurrentApi(urlService.NoteUrls.Note);
                      baseService.setIsLoadDataFromUrls(true);
                      baseService.setUrls([]);
                      baseService.setIsLoadPagingData(true);
                      baseService.setDataStatus("Active");
                      baseService.setCallerStatus(false);
                      $controller("ControllerBase", { $scope: $scope });
                  };

                  var init = function () {
                      $scope.addCall(urlService.NoteUrls.Note);
                
                      var dataProperties = ["list"];

                      baseService.setUrls($scope.urls);
                      baseService.setDataProperties(dataProperties);
                      baseService.setCallerStatus(true);
                      $scope.baseCaller();
                      $scope.start();
                  };            

                  $scope.start = function () {
                      $scope.model.Comment = null;
                      $scope.model.Remark = null;
                  };


                 config();
                 init()

            }
        ]);
          
  
  

10. Finally, create the HTML view for this controller in Client/app/views/notes/note.tpl.html and add the routing configuration in a new file Client/app/scripts/notes/note.config

        angular.module("app")
          .config([
              "$stateProvider", "$urlRouterProvider",
              function($stateProvider, $urlRouterProvider) {
                "use strict";

                  $stateProvider
                    .state("root.note",
                        {
                            url: "/note",
                            data: { pageTitle: "Note" },
                            views: {
                              "": {
                                templateUrl: "app/views/notes/note.tpl.html",
                                controller: "NoteController"
                            }
                        }

                    });
            }
        ]);
        


11. One lase action has remained, need to create permission in StaticResource.cs for this note


            public class Note
            {
                public static StaticResourceDto NoteParent =
                    new StaticResourceDto(ResourceOwner.Tenant, "Note", "Note", "root.note", null, false, 11);

                public static StaticResourceDto CompanyNote = new StaticResourceDto
                    (ResourceOwner.Tenant, "CompanyNote", "Company Note", "root.note");

               
                public static List<StaticResourceDto> GetAll()
                {
                    var list = new List<StaticResourceDto>
                    {
                        NoteParent,
                        CompanyNote.AddParent(NoteParent),
                    };

                    list.AddRange(CompanyNote.AddCrudChildren());

                    return list;
                }
            }
            
            
            public static List<StaticResourceDto> GetAllPrivateResources()
            {
                .........
                list.AddRange(Note.GetAll());


                return list;
            }
        }




If you can do it for one model. Hopefully from the next model, you can do a CRUD for a simple model within a very quick time through the help of copy-pasting.
