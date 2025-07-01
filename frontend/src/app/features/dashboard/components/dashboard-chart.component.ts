import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';

@Component({
  selector: 'app-dashboard-chart',
  standalone: true,
  imports: [CommonModule, ChartModule],
  template: `
    <div class="chart-container">
      <h5>Kullanıcı Aktivite Dağılımı</h5>
      <p-chart type="pie" [data]="userActivityData" [options]="chartOptions"></p-chart>
    </div>
  `,
  styles: [`
    .chart-container {
      margin-top: 2rem;
    }
  `]
})
export class DashboardChartComponent implements OnInit {
  userActivityData: any;
  chartOptions: any;

  ngOnInit(): void {
    this.initChartData();
    this.setupChartOptions();
  }

  private setupChartOptions(): void {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    this.chartOptions = {
        plugins: {
            legend: {
                labels: {
                    usePointStyle: true,
                    color: textColor
                }
            }
        }
    };
  }

  private initChartData(): void {
    const documentStyle = getComputedStyle(document.documentElement);
    this.userActivityData = {
        labels: ['Kullanıcı Yönetimi', 'Rol Yönetimi', 'Sistem Ayarları', 'Raporlama', 'Log İnceleme'],
        datasets: [
            {
                data: [540, 325, 702, 421, 869],
                backgroundColor: [
                    documentStyle.getPropertyValue('--blue-500'),
                    documentStyle.getPropertyValue('--yellow-500'),
                    documentStyle.getPropertyValue('--green-500'),
                    documentStyle.getPropertyValue('--purple-500'),
                    documentStyle.getPropertyValue('--orange-500')
                ],
                hoverBackgroundColor: [
                    documentStyle.getPropertyValue('--blue-400'),
                    documentStyle.getPropertyValue('--yellow-400'),
                    documentStyle.getPropertyValue('--green-400'),
                    documentStyle.getPropertyValue('--purple-400'),
                    documentStyle.getPropertyValue('--orange-400')
                ]
            }
        ]
    };
  }
} 