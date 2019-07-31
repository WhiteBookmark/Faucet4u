<template>
    <div>
        <mdb-row>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col class="text-center">
                <mdb-row>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                    <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
                </mdb-row>
                <br />
                <br />
                <ag-grid-vue style="width: 700px"
                             class="ag-theme-blue mx-auto"
                             :gridOptions="gridOptions"
                             @grid-ready="onGridReady"
                             :columnDefs="columnDefs"
                             :defaultColDef="defaultColDef"
                             :rowData="rowData"
                             :pagination="true">
                </ag-grid-vue>
            </mdb-col>
            <mdb-col col="1">
                <skyscrapper-banner></skyscrapper-banner>
            </mdb-col>
            <mdb-col col="1"></mdb-col>
        </mdb-row>

        <mdb-row>
            <mdb-col>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </mdb-col>
            <mdb-col>
                <standard-banner></standard-banner><br />
                <standard-banner></standard-banner><br />
            </mdb-col>
        </mdb-row>

        <button type="button"
                class="bottomLeftButton"
                id="bottomLeftButton"
                onClick="document.getElementById('bottomLeftBanner').remove();this.remove();">
            x
        </button>
        <div class="bottomLeftBanner" id="bottomLeftBanner">
            <square-banner></square-banner>
        </div>

        <button type="button"
                class="bottomRightButton"
                id="bottomRightButton"
                onClick="document.getElementById('bottomRightBanner').remove();this.remove();">
            x
        </button>
        <div class="bottomRightBanner" id="bottomRightBanner">
            <square-banner></square-banner>
        </div>
    </div>
</template>

<script>
    import { mdbRow, mdbCol, Btn } from "mdbvue";

    export default {
        components: { mdbRow, mdbCol, Btn },
        data()
        {
            return {
                gridOptions: null,
                gridApi: null,
                columnApi: null,
                columnDefs: null,
                defaultColDef: null,
                rowData: null
            };
        },
        methods: {
            onGridReady(params)
            {
                this.axios
                    .get(process.env.VUE_APP_API_History_Deposit, {
                        params: { sessionId: this.$cookie.get("sessionId") }
                    })
                    .then(response =>
                    {
                        this.rowData = response.data;
                        this.gridApi.sizeColumnsToFit();
                        this.gridApi.setDomLayout("autoHeight");
                    })
                    .catch(error =>
                    {
                        this.$store.commit("errorMessage", error.response.data);
                        this.$store.commit("errorModal", true);
                    });
            }
        },
        beforeMount()
        {
            this.gridOptions = {};
            this.defaultColDef = { resizable: true, sortable: true };
            this.columnDefs = [

                { headerName: 'Deposit Date', field: 'DepositDate', sort: 'desc' },
                { headerName: 'Amount', field: 'Amount', valueFormatter: this.bitcoinAgGridFormat },
                { headerName: 'Confirmations', field: 'Confirmations' },
                { headerName: 'Admin Comment', field: 'Remarks' }
            ];
        },
        mounted()
        {
            this.gridApi = this.gridOptions.api;
            this.gridColumnApi = this.gridOptions.columnApi;
        }
    };
</script>