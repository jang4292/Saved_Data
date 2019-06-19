package com.bpm202.SensorProject.Main.History;

import android.content.Context;
import android.support.annotation.NonNull;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.TextView;

import com.bpm202.SensorProject.BaseFragment;
import com.bpm202.SensorProject.Data.ExDataSrouce;
import com.bpm202.SensorProject.Data.ExRepository;
import com.bpm202.SensorProject.R;
import com.bpm202.SensorProject.Util.Util;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.Objects;

public class HistoryCalendarFragment1 extends BaseFragment {

    private TextView mTvYear;
    private TextView mTvMonth;
    private GridView mGridView;

    private int mMonth;
    private int mYear;

    private final Date date = new Date(System.currentTimeMillis());
    private static final int FIRST_DAY = 1;

    private Calendar mCalendar;

    private static HistoryCalendarFragment1 instance = null;

    public static HistoryCalendarFragment1 newInstance() {
        return instance == null ? new HistoryCalendarFragment1() : instance;
    }

    @NonNull
    @Override
    protected int getLayoutId() {
        return R.layout.fragment_history_calendar11;
    }

    @NonNull
    @Override
    protected void initView(View v) {
        mTvYear = v.findViewById(R.id.tv_year);
        mTvMonth = v.findViewById(R.id.tv_month);
        mGridView = v.findViewById(R.id.gridview_days);

        // Listener setting
        v.findViewById(R.id.ibtn_prev).setOnClickListener(v12 -> {
            if (mMonth > 1) {
                mMonth--;
            } else {
                mMonth = 12;
                mYear--;
            }

            String year = mYear + getString(R.string.year);
            String month = mMonth + getString(R.string.month);
            mTvYear.setText(year);
            mTvMonth.setText(month);


            onLoaded();
        });
        v.findViewById(R.id.ibtn_next).setOnClickListener(v1 -> {
            if (mMonth < 12) {
                mMonth++;
            } else {
                mMonth = 1;
                mYear++;
            }

            String year = mYear + getString(R.string.year);
            String month = mMonth + getString(R.string.month);
            mTvYear.setText(year);
            mTvMonth.setText(month);

            onLoaded();
        });

        mCalendar = today();

        String year = mYear + getString(R.string.year);
        String month = mMonth + getString(R.string.month);
        mTvYear.setText(year);
        mTvMonth.setText(month);

        onLoaded();
    }

    // 현재 달의 캘린더 생성
    private Calendar today() {
        final SimpleDateFormat curYearFormat = new SimpleDateFormat("yyyy", Locale.KOREA);
        final SimpleDateFormat curMonthFormat = new SimpleDateFormat("MM", Locale.KOREA);
        Calendar calendar = (Calendar) Util.CalendarInfo.getCalendar().clone();

        calendar.set(
                Integer.parseInt(curYearFormat.format(date)),
                Integer.parseInt(curMonthFormat.format(date)) - 1,
                FIRST_DAY);

        mYear = Integer.parseInt(curYearFormat.format(date));
        mMonth = Integer.parseInt(curMonthFormat.format(date));

        return calendar;
    }

    // 서버에서 데이터 다운로드
    private void onLoaded() {
        Util.FadeAnimation.fadeOut(mGridView);
        ExRepository.getInstance().getExerciseMonth(String.valueOf(mYear), String.valueOf(mMonth),
                new ExDataSrouce.GetCallback() {
                    @Override
                    public void onLoaded(List<Integer> i_list) {
                        GridAdapter adapter = new GridAdapter(getContext(), updateDate(i_list));
                        mGridView.setAdapter(adapter);
                        Util.FadeAnimation.fadeIn(mGridView);
                    }

                    @Override
                    public void onDataNotAvailable() {
                        Util.FadeAnimation.fadeIn(mGridView);
                    }
                });
    }

    // 달력 내용 데이터 갱신
    private List<String> updateDate(List<Integer> exObjList) {
        String[] strs = Objects.requireNonNull(getContext()).getResources().getStringArray(R.array.day_of_week_list);
        List<String> list = new ArrayList<>(Arrays.asList(strs));

        mCalendar.set(mYear, mMonth - 1, FIRST_DAY);
        int dayNum = mCalendar.get(Calendar.DAY_OF_WEEK);

        for (int i = 1; i < dayNum; i++) {
            list.add("");
        }

        List<String> days = new ArrayList<>();
        for (int i = 1; i < 31; i++) {
            days.add("X");
        }


        for (int i = 0; i < exObjList.size(); i++) {
            Integer integer = exObjList.get(i);
            days.set(integer - 1, "O");
        }
        list.addAll(days);

        return list;
    }

    // 달력 데이터가 들어가는 그리드 어뎁터
    private class GridAdapter extends BaseAdapter {
        private List<String> list;
        private LayoutInflater inflater;
        private Context mContext;

        public GridAdapter(Context context, List<String> list) {
            this.mContext = context;
            this.inflater = (LayoutInflater) mContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            this.list = list;
        }


        @Override
        public int getCount() {
            return list.size();
        }

        @Override
        public Object getItem(int position) {
            return list.get(position);
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            ViewHolder holder;
            if (convertView == null) {
                convertView = inflater.inflate(R.layout.item_calendar, parent, false);

                holder = new ViewHolder();
                holder.tvDay = convertView.findViewById(R.id.tv_day);

                convertView.setTag(holder);
            } else {
                holder = (ViewHolder) convertView.getTag();
            }

            holder.tvDay.setText(list.get(position));
            holder.tvDay.setOnClickListener(v -> {
                Util.FragmentUtil.addFragmentToActivity(getFragmentManager(), HistoryDateFragment.newInstance(), R.id.child_fragment_container);
            });

            return convertView;
        }


        class ViewHolder {
            TextView tvDay;
        }
    }
}
