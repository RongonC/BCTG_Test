using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Models;
using StemmonsMobile.Views.Cases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;

namespace StemmonsMobile.ViewModels
{
    public class GroupedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region MyRegion Props

        private string _casetypeid;

        public string Casetypeid
        {
            get => _casetypeid;
            set
            {
                _casetypeid = value;
            }
        }

        private string _caseOwnerSam;

        public string CaseOwnerSam
        {
            get => _caseOwnerSam;
            set
            {
                _caseOwnerSam = value;
            }
        }
        private string _caseAssgnSam;
        public string CaseAssgnSam
        {
            get => _caseAssgnSam;
            set
            {
                _caseAssgnSam = value;
            }
        }

        private string _caseClosebySam;
        public string CaseCloseBySam
        {
            get => _caseClosebySam;
            set
            {
                _caseClosebySam = value;
            }
        }

        private string _casecreatebysam;
        public string CaseCreateBySam
        {
            get => _casecreatebysam;
            set
            {
                _casecreatebysam = value;
            }
        }
        private string _propertyid;
        public string PropertyId
        {
            get => _propertyid;
            set
            {
                _propertyid = value;
            }
        }
        private string _tenantcode;
        public string TenantCode
        {
            get => _tenantcode;
            set
            {
                _tenantcode = value;
            }
        }

        private string _tenant_id;
        public string TenantId
        {
            get => _tenant_id;
            set
            {
                _tenant_id = value;
            }
        }

        private string _showOpenClosetype;
        public string ShowOpenCloseType
        {
            get => _showOpenClosetype;
            set
            {
                _showOpenClosetype = value;
            }
        }
        private string _showpastcase;
        public string ShowPastCase
        {
            get => _showpastcase;
            set
            {
                _showpastcase = value;
            }
        }
        private string _searchquery;
        public string SearchQuery
        {
            get => _searchquery;
            set
            {
                _searchquery = value;
            }
        }
        private string _sTitle;
        public string STitle
        {
            get => _sTitle;
            set
            {
                _sTitle = value;
            }
        }
        private bool _saveRec;
        public bool SaveRec
        {
            get => _saveRec;
            set
            {
                _saveRec = value;
            }
        }
        private string _scrnName;
        public string ScrnName
        {
            get => _scrnName;
            set
            {
                _scrnName = value;
            }
        }

        private int? _entityid = 0;
        public int? EntityID
        {
            get => _entityid;
            set
            {
                _entityid = value;
            }
        }

        private int? _pageIndex = 1;
        public int? PageIndex
        {
            get => _pageIndex;
            set
            {
                _pageIndex = value;
            }
        }

        private int? _pagenumber;

        public int? PageNumber
        {
            get => _pagenumber;
            set
            {
                _pagenumber = value;
            }
        }
        #endregion

        public GroupedViewModel()
        {

            try
            {
                Master_list = new ObservableCollection<Group_Caselist>();
                //{
                //    OnLoadMore = async () =>
                //    {

                //        //load the next page
                //        await GetCaseListWithCall();
                //        return null;

                //    }
                //};

                TempList = new ObservableCollection<Group_Caselist>();
                _headerTapCommand = new Command<object>(HeaderTapped);
            }
            catch (Exception ex)
            {
            }

            #region InfiniteScroll Class Implementation
            //this.LoadDataCommand = new Command(async () =>
            //{
            //    PageIndex += 1;
            //    await this.GetCaseListWithCall();

            //    //if (Master_list?.FirstOrDefault().Count - 2 <= PageIndex)
            //    //{
            //    //    if (Master_list?.FirstOrDefault().Count == (PageNumber * PageIndex))
            //    //    {
            //    //        await this.GetCaseListWithCall();
            //    //        PageIndex += 1;
            //    //    }
            //    //}
            //});
            #endregion
        }

        public async Task GetCaseListByEntityID()
        {

            await Task.Run(() =>
            {
                List<BasicCase> GetCaseList = null;
                var res = CasesAPIMethods.GetCaseListRelationDatabyentityid(Convert.ToInt32(Casetypeid), Functions.UserName);
                var result = res.GetValue("ResponseContent").ToString();
                GetCaseList = JsonConvert.DeserializeObject<List<BasicCase>>(result);

                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        BasicCase_lst = new ObservableCollection<BasicCase>(GetCaseList);
                        var tm = BasicCase_lst.Select(i =>
                        {
                            if (!string.IsNullOrEmpty(i.strCaseDue))
                            {
                                i.bg_color = Convert.ToDateTime(CommonConstants.DateFormatStringToString(i.strCaseDue)) > DateTime.Now ? "Black" : "Red";
                                i.DueDateVisibility = true;
                            }
                            else
                            {
                                i.bg_color = "Black";
                                i.DueDateVisibility = false;
                            }
                            return i;
                        });

                        BasicCase_lst = new ObservableCollection<BasicCase>(tm);
                    }
                    catch
                    {
                        if (GetCaseList != null)
                        {
                            BasicCase_lst = new ObservableCollection<BasicCase>(GetCaseList);
                        }
                    }
                });
            });

            Master_list.Clear();

            Master_List_Function(BasicCase_lst);
        }

        #region Pull To refresh Case List
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    try
                    {
                        await Task.Run(() =>
                        {
                            dynamic result = null;
                            PageIndex = 0;
                            result = GetCaseListWithCall();
                            IsRefreshing = false;
                            // Device.BeginInvokeOnMainThread(() =>
                            // {
                            //     try
                            //     {
                            //         IsRefreshing = false;
                            //    //  WarningLabel.IsVisible = false;
                            //    if (result.Result.Count > 0)
                            //         {
                            //             var t = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
                            //             BasicCase_lst = t;
                            //         }
                            //         Master_List_Function(BasicCase_lst);
                            //    //this.listdata.ItemsSource = BasicCase_lst;
                            //}
                            //     catch (Exception ex)
                            //     {
                            //         string exStr = ex.Message;
                            //     }
                            // });
                        });

                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
        #endregion

        Command _headerTapCommand;
        public Command HeaderTapCommand
        {
            get
            {
                return _headerTapCommand;
            }
        }

        //public ICommand LoadDataCommand
        //{
        //    get;
        //    set;
        //} 

        public ObservableCollection<Group_Caselist> _expandedGroups = new ObservableCollection<Group_Caselist>();

        public ObservableCollection<BasicCase> BasicCase_lst = new ObservableCollection<BasicCase>();
        private ObservableCollection<Group_Caselist> _Master_list;
        public ObservableCollection<Group_Caselist> TempList;

        public ObservableCollection<Group_Caselist> Master_list
        {
            get => _Master_list;
            set
            {
                if (value != null)
                {
                    _Master_list = value;
                    //onItemTap_Header(Master_list);
                    OnPropertyChanged(nameof(Master_list));
                }
            }
        }
        public async void onItemTap(object item)
        {
            var tap = item as BasicCase;
            await Application.Current.MainPage.Navigation.PushAsync(new ViewCasePage(Convert.ToString(tap.CaseID), Convert.ToString(tap.CaseTypeID), tap.CaseTypeName, ""));
        }

        BasicCase _clist;
        public BasicCase _clist_property
        {
            get => _clist;
            set
            {
                if (value != null)
                {
                    _clist = value;
                    onItemTap(_clist);
                }
            }
        }

        public async Task GetCaseListWithCall()
        {

            await Task.Run(() =>
            {
                var result = CasesSyncAPIMethods.GetCaseList(App.Isonline, Functions.UserName, _casetypeid, _caseOwnerSam, _caseAssgnSam, _caseClosebySam, _casecreatebysam, _propertyid, _tenantcode, _tenant_id, _showOpenClosetype, _showpastcase, _searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, _saveRec, _scrnName, PageIndex, PageNumber);
                result.Wait();

                Device.BeginInvokeOnMainThread(() =>
                {
                    BasicCase_lst = new ObservableCollection<BasicCase>(result.Result);

                    var tm = BasicCase_lst.Select(i =>
                    {
                        if (!string.IsNullOrEmpty(i.strCaseDue))
                        {
                            i.bg_color = Convert.ToDateTime(CommonConstants.DateFormatStringToString(i.strCaseDue)) > DateTime.Now ? "Black" : "Red";
                            i.DueDateVisibility = true;
                        }
                        else
                        {
                            i.bg_color = "Black";
                            i.DueDateVisibility = false;
                        }
                        return i;
                    });

                    BasicCase_lst = new ObservableCollection<BasicCase>(tm);
                });
            });

            Master_list.Clear();

            Master_List_Function(BasicCase_lst);
        }

        public void HeaderTapped(object obj)
        {
            try
            {
                int selectedIndex = _expandedGroups.IndexOf(
                ((Group_Caselist)(obj)));
                Master_list[selectedIndex].Expanded = !Master_list[selectedIndex].Expanded;
                UpdateListContent();
            }
            catch (Exception ex)
            {
            }
        }

        private void Master_List_Function(ObservableCollection<BasicCase> basicCase_Item)
        {
            try
            {
                Master_list.Clear();
                TempList.Clear();
                var Grp = basicCase_Item.GroupBy(v => v.CaseTypeName);
                foreach (var item in Grp)
                {
                    Group_Caselist fav_case = new Group_Caselist(item.Key);
                    foreach (var ite in item)
                    {
                        fav_case.Add(ite);
                    }
                    TempList.Add(fav_case);
                    Master_list.Add(fav_case);
                    Debug.WriteLine(Master_list);
                }
                UpdateListContent();

            }
            catch (Exception ex)
            {


            }

        }

        public void UpdateListContent()
        {
            try
            {
                _expandedGroups = new ObservableCollection<Group_Caselist>();
                for (int i = 0; i < Master_list.Count; i++)
                {
                    Group_Caselist group = Master_list[i];
                    //Create new Groups so we do not alter original list
                    Group_Caselist newGroup = new Group_Caselist(group.Title, group.Expanded);
                    //Add the count of items for Lits Header Titles to use

                    if (group.Expanded)
                    {
                        var tem = TempList[i];
                        foreach (BasicCase ite in tem)
                        {
                            newGroup.Add(ite);
                        }
                    }
                    _expandedGroups.Add(newGroup);
                }
                Master_list.Clear();
                Master_list = _expandedGroups;

            }
            catch (Exception)
            {
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}