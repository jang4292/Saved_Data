package com.bpm202.SensorProject.Main.Schedules;

import android.support.v4.app.Fragment;

import com.bpm202.SensorProject.Data.CommonData;

import java.util.ArrayList;
import java.util.Observable;
import java.util.Observer;

public class SchdulesManager {

    private static SchdulesManager instance = null;
    private static StateChangedObservable observable = null;

    private ArrayList<SchedulesViewPagerFragment> viewPagerFragmentList = null;
    private ArrayList<SchedulesAddViewPagerFragment> addViewPagerFragment = null;

    public static SchdulesManager Instance() {
        return (instance == null) ? new SchdulesManager() : instance;
    }

    private ArrayList<SchedulesViewPagerFragment> getViewPagerFragmentList() {
        if (viewPagerFragmentList == null) {
            viewPagerFragmentList = new ArrayList<>();
            for (int i = 0; i < CommonData.WEEK_DATA_LIST.length; i++) {
                viewPagerFragmentList.add(SchedulesViewPagerFragment.Instance());
            }
        }
        return viewPagerFragmentList;
    }

    private ArrayList<SchedulesAddViewPagerFragment> getAddViewPagerFragment() {
        if (addViewPagerFragment == null) {
            addViewPagerFragment = new ArrayList<>();
            for (int i = 0; i < CommonData.TRAIN_LIST.length; i++) {
                addViewPagerFragment.add(SchedulesAddViewPagerFragment.Instance());
            }
        }
        return addViewPagerFragment;
    }

    ArrayList<Fragment> initViewPager() {
        ArrayList<Fragment> fl = new ArrayList<>();
        if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.DEFAULT)) {
            for (int i = 0; i < CommonData.WEEK_DATA_LIST.length; i++) {
                SchedulesViewPagerFragment viewPagerFragment = SchdulesManager.Instance().getViewPagerFragmentList().get(i);
                fl.add(viewPagerFragment);
            }
        } else if (SchdulesManager.Instance().getSTATE().equals(SchdulesManager.STATE.ADD)) {
            for (int i = 0; i < CommonData.TRAIN_LIST.length; i++) {
                SchedulesAddViewPagerFragment viewPagerFragment = SchdulesManager.Instance().getAddViewPagerFragment().get(i);
                fl.add(viewPagerFragment);
            }
        }
        return fl;
    }

    public void addObserver(Observer o) {
        if (observable == null) {
            observable = new StateChangedObservable();
        }
        observable.addObserver(o);
    }

    public void deleteObserver(Observer o) {
        if (observable != null) {
            observable.deleteObserver(o);
            if (observable.countObservers() < 1) {
                observable = null;
            }
        }
    }

    public enum STATE {ADD, MODIFY, DEFAULT, RELOAD}

    private static STATE state = STATE.DEFAULT;

    public void setSTATE(STATE state) {
        this.state = state;
        if (observable != null) {
            observable.notifyObservers();
        }
    }

    public STATE getSTATE() {
        return state;
    }

    private static int currentPage = 0;

    public void setCurrentPageOfDay(int i) {
        currentPage = i;
    }

    public int getCurrentPageOfDay() {
        return currentPage;
    }

    private static class StateChangedObservable extends Observable {
        @Override
        public void notifyObservers() {
            setChanged();
            super.notifyObservers();
        }
    }
}
