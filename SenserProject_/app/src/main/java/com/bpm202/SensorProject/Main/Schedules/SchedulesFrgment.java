package com.bpm202.SensorProject.Main.Schedules;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentStatePagerAdapter;
import android.support.v4.view.ViewPager;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;

import com.bpm202.SensorProject.CustomView.CustomViewPager;
import com.bpm202.SensorProject.Data.CommonData;
import com.bpm202.SensorProject.Data.ScheduleDataSource;
import com.bpm202.SensorProject.Data.ScheduleRepository;
import com.bpm202.SensorProject.Main.MainActivity;
import com.bpm202.SensorProject.R;
import com.bpm202.SensorProject.Util.Util;
import com.bpm202.SensorProject.ValueObject.DayOfWeek;
import com.bpm202.SensorProject.ValueObject.ScheduleValueObject;

import java.util.ArrayList;
import java.util.List;
import java.util.Observable;
import java.util.Observer;

public class SchedulesFrgment extends Fragment implements Observer {

    private static SchedulesFrgment instance = null;
    private ViewPagerAdapter viewPagerAdapter;

    public static SchedulesFrgment Instance() {
        if (instance == null) {
            instance = new SchedulesFrgment();
        }
        return instance;
    }

    public static void DestroyInstance() {
        if (instance != null) {
            instance = null;
        }
    }

    private TabLayout tabLayout;
    private CustomViewPager view_pager;


    @Override
    public void onAttach(Context context) {
        SchdulesManager.Instance().addObserver(this);
        super.onAttach(context);
    }

    @Override
    public void onDestroyView() {
        SchdulesManager.Instance().deleteObserver(this);
        super.onDestroyView();
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_schedules, container, false); // 여기서 UI를 생성해서 View를 return
        initView(v);
        return v;
    }

    private void initView(View v) {
        tabLayout = v.findViewById(R.id.tab_layout);
        view_pager = v.findViewById(R.id.view_pager);

        viewPagerAdapter = new ViewPagerAdapter(getFragmentManager());
        view_pager.setAdapter(viewPagerAdapter);
        view_pager.addOnPageChangeListener(new TabLayout.TabLayoutOnPageChangeListener(tabLayout));
        view_pager.addOnPageChangeListener(new ViewPager.OnPageChangeListener() {
            @Override
            public void onPageScrolled(int i, float v, int i1) {
                SchdulesManager.Instance().setCurrentPageOfDay(i);
            }

            @Override
            public void onPageSelected(int i) {

            }

            @Override
            public void onPageScrollStateChanged(int i) {

            }
        });

    }

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        loadSchedulesData();
        super.onActivityCreated(savedInstanceState);
    }

    private List<ScheduleValueObject> dataList;

    private void loadSchedulesData() {
        ScheduleRepository repository = ScheduleRepository.getInstance();
        Util.LoadingProgress.show(getContext());
        repository.getSchedules(new ScheduleDataSource.LoadCallback() {
            @Override
            public void onLoaded(List<ScheduleValueObject> scheduleVos) {
                Util.LoadingProgress.hide();
                if (scheduleVos == null) {
                    Log.e(MainActivity.TAG, "[TEST] , Data is null");
                } else {
                    dataList = scheduleVos;
                    SchdulesManager.Instance().setSTATE(SchdulesManager.STATE.DEFAULT);
                }
            }

            @Override
            public void onDataNotAvailable() {
                Util.LoadingProgress.hide();
                Log.d("TEST", "TEST, SchedulesFrgment onDataNotAvailable");
            }
        });
    }

    @Override
    public void update(Observable o, Object arg) {
        notifyDateSetChanged();
    }

    private void notifyDateSetChanged() {
        if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.RELOAD)) {
            loadSchedulesData();
        } else if (!SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.MODIFY)) {
            initTapControl();
            initViewPager();
            view_pager.setPagingEnabled(true);
        } else {
            view_pager.setPagingEnabled(false);
        }
        getActivity().invalidateOptionsMenu();
    }


    private void initViewPager() {
        ArrayList<Fragment> fl = SchdulesManager.Instance().initViewPager();
        if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.DEFAULT)) {
            for (int i = 0; i < CommonData.WEEK_DATA_LIST.length; i++) {
                DayOfWeek[] dayOfWeek = DayOfWeek.values();
                List<ScheduleValueObject> objs = new ArrayList<>();
                for (ScheduleValueObject obj : dataList) {
                    if (obj.getDay() == dayOfWeek[i]) {
                        objs.add(obj);
                    }
                }
                Fragment fragment = fl.get(i);
                if (fragment instanceof SchedulesViewPagerFragment) {
                    ((SchedulesViewPagerFragment) fragment).setData(objs);
                }
            }
        } else if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.ADD)) {
            for (int i = 0; i < CommonData.TRAIN_LIST.length; i++) {
                Fragment fragment = fl.get(i);
                if (fragment instanceof SchedulesAddViewPagerFragment) {
                    ((SchedulesAddViewPagerFragment) fragment).setTabPositionFromParent(i);
                }
            }
        }

        viewPagerAdapter.notifyDataSetChanged(fl);
        view_pager.setOffscreenPageLimit(fl.size());

        if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.DEFAULT)) {
            view_pager.setCurrentItem(SchdulesManager.Instance().getCurrentPageOfDay());
        }
    }

    private void initTapControl() {
        if (tabLayout != null) {
            tabLayout.removeAllTabs();
            tabLayout.removeOnTabSelectedListener(onTabSelectedListener);
        }

        if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.DEFAULT)) {
            for (String dayName : CommonData.WEEK_DATA_LIST) {
                tabLayout.addTab(tabLayout.newTab().setText(dayName));
            }
        } else if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.ADD)) {
            for (String trainName : CommonData.TRAIN_LIST) {
                tabLayout.addTab(tabLayout.newTab().setText(trainName));
            }
        }
        tabLayout.addOnTabSelectedListener(onTabSelectedListener);
    }

    private TabLayout.OnTabSelectedListener onTabSelectedListener = new TabLayout.OnTabSelectedListener() {
        @Override
        public void onTabSelected(TabLayout.Tab tab) {
            int position = tab.getPosition();
            view_pager.setCurrentItem(position);
        }

        @Override
        public void onTabUnselected(TabLayout.Tab tab) {
        }

        @Override
        public void onTabReselected(TabLayout.Tab tab) {
        }
    };

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.schedules_edit_menu, menu);
    }

    @Override
    public void onPrepareOptionsMenu(Menu menu) {
        if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.DEFAULT)) {
            menu.getItem(0).setVisible(true);
            menu.getItem(1).setVisible(false);
        } else if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.ADD)
                || SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.MODIFY)) {
            menu.getItem(0).setVisible(false);
            menu.getItem(1).setVisible(true);
        } else {
            menu.getItem(0).setVisible(true);
            menu.getItem(1).setVisible(true);
        }
        super.onPrepareOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.action_modify:
                break;
            case R.id.menu_modify:
                SchdulesManager.Instance().setSTATE(SchdulesManager.STATE.MODIFY);
                break;
            case R.id.menu_add:
                SchdulesManager.Instance().setSTATE(SchdulesManager.STATE.ADD);
                break;
            case R.id.action_add:
                SchdulesManager.Instance().setSTATE(SchdulesManager.STATE.DEFAULT);
                break;
        }
        return super.onOptionsItemSelected(item);
    }

    private class ViewPagerAdapter extends FragmentStatePagerAdapter {

        @NonNull
        private ArrayList<Fragment> fragmentList;

        public ViewPagerAdapter(@NonNull FragmentManager fm) {
            super(fm);
        }

        public void notifyDataSetChanged(@NonNull ArrayList<Fragment> fragmentList) {
            this.fragmentList = fragmentList;
            super.notifyDataSetChanged();

        }

        @Override
        public int getItemPosition(@NonNull Object object) {
            return super.getItemPosition(object);
        }

        @Override
        public Fragment getItem(int i) {
            Log.i(MainActivity.TAG, "ViewPagerAdapter getItem position : " + i);
            return fragmentList != null ? fragmentList.get(i) : null;
        }

        @Override
        public int getCount() {
            return fragmentList != null ? fragmentList.size() : 0;
        }
    }
}
