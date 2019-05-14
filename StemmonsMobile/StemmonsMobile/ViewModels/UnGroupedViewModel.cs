using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Views.Cases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StemmonsMobile.ViewModels.CaseListViewModels
{
    public class UnGroupedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool LoadMoreFlag = false;
        public bool LoadAllFlag = false;
        public List<int?> PageIndexList;

        //public static readonly BindableProperty IsWorkingProperty =
        //     BindableProperty.Create(nameof(IsWorking), typeof(bool), typeof(ViewModel_1), default(bool));

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

        private static int? _pageIndex = 1;
        public static int? PageIndex
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

        public UnGroupedViewModel()
        {
            PageIndex = 1;
            PageIndexList = new List<int?>() { 0 };
            BasicCase_lst = new ObservableCollection<GetCaseTypesResponse.BasicCase>();
            TempList = new ObservableCollection<GetCaseTypesResponse.BasicCase>();
            //_headerTapCommand = new Command<object>(HeaderTapped);

            //Master_list.LoadMoreAsync();
            //IsRefreshing = false;

            #region InfiniteScroll Class Implementation
            this.LoadDataCommand = new Command(async () =>
            {
                IsLoading = true;
                LoadMoreFlag = true;
                PageIndex += 1;
                if (!LoadAllFlag)
                {
                    await GetCaseListWithCall();
                }
                IsLoading = false;
            });
            #endregion
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading == value)
                    return;

                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
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
                    LoadMoreFlag = false;
                    LoadAllFlag = true;
                    IsRefreshing = true;
                    PageIndex = 0;
                    await GetCaseListWithCall();
                    IsRefreshing = false;


                });
            }
        }
        #endregion

        public ICommand LoadDataCommand
        {
            get;
            set;
        }

        public ObservableCollection<GetCaseTypesResponse.BasicCase> TempList;

        public ObservableCollection<GetCaseTypesResponse.BasicCase> _basiccase_lst;

        public ObservableCollection<GetCaseTypesResponse.BasicCase> BasicCase_lst
        {
            get => _basiccase_lst;
            set
            {
                if (value != null)
                {
                    _basiccase_lst = value;
                    OnPropertyChanged(nameof(BasicCase_lst));
                }
            }
        }
        public async void onItemTap(object item)
        {
            if (item != null)
            {
                var tap = item as GetCaseTypesResponse.BasicCase;
                await Application.Current.MainPage.Navigation.PushAsync(new ViewCasePage(Convert.ToString(tap.CaseID), Convert.ToString(tap.CaseTypeID), tap.CaseTypeName, ScrnName));
            }
        }

        GetCaseTypesResponse.BasicCase _clist;
        public GetCaseTypesResponse.BasicCase _clist_property
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

            try
            {
                //if (PageIndexList.Contains(PageIndex) && PageIndex != 0)
                //{
                //    return;
                //}
                //else
                //{
                PageIndexList.Add(PageIndex);
                //PageIndexList.RemoveAt(PageIndexList.Count - 1);
                await Task.Run(() =>
                {
                    var result = CasesSyncAPIMethods.GetCaseList(App.Isonline, Functions.UserName, _casetypeid, _caseOwnerSam, _caseAssgnSam, _caseClosebySam, _casecreatebysam, _propertyid, _tenantcode, _tenant_id, _showOpenClosetype, _showpastcase, _searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, _saveRec, _scrnName, PageIndex, PageNumber);
                    result.Wait();


                    if (!LoadMoreFlag) //Refresh or default
                        {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            BasicCase_lst = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
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

                            BasicCase_lst = new ObservableCollection<GetCaseTypesResponse.BasicCase>(tm);
                        });
                    }
                    else //LoadMore
                        {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TempList.Clear();
                            TempList = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
                            var tm = TempList.Select(i =>
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

                            TempList = new ObservableCollection<GetCaseTypesResponse.BasicCase>(tm);

                            foreach (var item in TempList)
                            {
                                BasicCase_lst.Add(item);
                            }
                        });
                    }
                });
                //}
            }
            catch (Exception ex)
            {

            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
