﻿using System;
using System.Globalization;
using System.Windows;
using DevExpress.Xpf.PivotGrid;

namespace DXPivotGrid_FindCells {
    public partial class Window1 : Window {
        public Window1() {
            InitializeComponent();
            pivotGrid.CustomFieldValueCells += 
                new PivotCustomFieldValueCellsEventHandler(pivotGrid_CustomFieldValueCells);
        }
        void Window_Loaded(object sender, RoutedEventArgs e) {
            PivotHelper.FillPivot(pivotGrid);
            pivotGrid.DataSource = PivotHelper.GetDataTable();
            pivotGrid.BestFit();
        }

        // Handles the CustomFieldValueCells event to remove columns with
        // zero summary values.
        void pivotGrid_CustomFieldValueCells(object sender, PivotCustomFieldValueCellsEventArgs e) {
            if (pivotGrid.DataSource == null) return;
            if (rbDefault.IsChecked == true) return;

            // Obtains the first encountered column header whose column
            // matches the specified condition, represented by a predicate.
            FieldValueCell cell = e.FindCell(true, new Predicate<object[]>(

                // Defines the predicate returning true for columns
                // that contain only zero summary values.
                delegate(object[] dataCellValues) {
                    foreach (object value in dataCellValues) {
                        if (!object.Equals((decimal)0, value))
                            return false;
                    }
                    return true;
            }));

            // If any column header matches the condition, this column is removed.
            if (cell != null) e.Remove(cell);
        }
        void pivotGrid_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e) {
            if(e.Field == pivotGrid.Fields[PivotHelper.Month]) {
                e.DisplayText = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)e.Value);
            }
        }
        private void rbDefault_Checked(object sender, RoutedEventArgs e) {
            pivotGrid.LayoutChanged();
        }
    }
}
