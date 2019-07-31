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

        <p class="font-weight-bold">
          We pay you for 3 levels of referrals deep. <br />
          We pay {{ this.$store.getters.faucetSettingValue("Level1").Value }}%
          to you from all your Level 1 earnings,<br />
          {{ this.$store.getters.faucetSettingValue("Level2").Value }}% from all
          your level 2 referrals earnings,<br />
          {{ this.$store.getters.faucetSettingValue("Level3").Value }}% to you
          from all your level 3 referrals earnings
        </p>

        <mdb-btn v-on:click="showLevel1()" color="secondary">Level 1</mdb-btn>
        <mdb-btn v-on:click="showLevel2()" color="secondary">Level 2</mdb-btn>
        <mdb-btn v-on:click="showLevel3()" color="secondary">Level 3</mdb-btn>
        <br />
        <br />

        <mdb-datatable
          v-bind:data="data"
          v-bind:searching="true"
          class="w-75"
          v-bind:pagination="true"
          striped
          bordered
          hover
          small
          autoWidth
          responsive
        />
      </mdb-col>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col col="1"></mdb-col>
    </mdb-row>

    <mdb-row>
      <mdb-col>
        <standard-banner></standard-banner>
        <br />
        <standard-banner></standard-banner>
        <br />
        <standard-banner></standard-banner>
        <br />
      </mdb-col>
      <mdb-col>
        <standard-banner></standard-banner>
        <br />
        <standard-banner></standard-banner>
        <br />
        <standard-banner></standard-banner>
        <br />
      </mdb-col>
    </mdb-row>

    <button
      type="button"
      class="bottomLeftButton"
      id="bottomLeftButton"
      onClick="document.getElementById('bottomLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomLeftBanner" id="bottomLeftBanner">
      <square-banner></square-banner>
    </div>

    <button
      type="button"
      class="bottomRightButton"
      id="bottomRightButton"
      onClick="document.getElementById('bottomRightBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomRightBanner" id="bottomRightBanner">
      <square-banner></square-banner>
    </div>
  </div>
</template>

<script>
import { mdbRow, mdbCol, mdbBtn, mdbDatatable } from "mdbvue";

export default {
  components: { mdbRow, mdbCol, mdbBtn, mdbDatatable },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      errorList: null,
      successList: null,
      reference: null,
      subject: null,
      message: null,
      data: {
        columns: [
          {
            label: "Username",
            field: "Username",
            sort: "asc"
          },
          {
            label: "Total Credited",
            field: "TotalCredited",
            sort: "asc"
          },
          {
            label: "Faucet Claim Earning",
            field: "LinkShortnerEarning",
            sort: "asc"
          },
          {
            label: "Offerwall Earning",
            field: "OfferwallEarning",
            sort: "asc"
          },
          {
            label: "PTP Unique Earning",
            field: "PTPUniqueEarning",
            sort: "asc"
          },
          {
            label: "PTP Non-Unique 1 Earning",
            field: "PTPNonUnique1Earning",
            sort: "asc"
          },
          {
            label: "PTP Non-Unique 2 Earning",
            field: "PTPNonUnique2Earning",
            sort: "asc"
          },
          {
            label: "PTP Non-Unique 3 Earning",
            field: "PTPNonUnique3Earning",
            sort: "asc"
          },
          {
            label: "Last Faucet Claim",
            field: "LastClaim",
            sort: "asc"
          }
        ],
        rows: []
      }
    };
  },
  sockets: {},
  methods: {
    test: function() {},
    showLevel1: function() {
      this.$store.commit("isLoading", true);

      this.data.rows.length = 0;
      var data = this.$store.state.level1Referrals;

      for (var i = 0; i < Object.keys(data).length; i++) {
        var totalEarning =
          (this.$store.getters.faucetSettingValue("Level1").Value / 100) *
            data[i].LinkShortnerEarning +
          (this.$store.getters.faucetSettingValue("PTPLevel1").Value / 100) *
            (data[i].PTPUniqueEarning +
              data[i].PTPNonUnique1Earning +
              data[i].PTPNonUnique2Earning +
              data[i].PTPNonUnique3Earning) +
          (this.$store.getters.faucetSettingValue("OfferwallLevel1").Value /
            100) *
            data[i].OfferwallEarning;
        totalEarning = totalEarning.toFixed(8);
        this.data.rows.push({
          Username: data[i].Username,
          TotalCredited: totalEarning,
          LinkShortnerEarning: data[i].LinkShortnerEarning.toFixed(8),
          OfferwallEarning: data[i].OfferwallEarning.toFixed(8),
          PTPUniqueEarning: data[i].PTPUniqueEarning.toFixed(8),
          PTPNonUnique1Earning: data[i].PTPNonUnique1Earning.toFixed(8),
          PTPNonUnique2Earning: data[i].PTPNonUnique2Earning.toFixed(8),
          PTPNonUnique3Earning: data[i].PTPNonUnique3Earning.toFixed(8),
          LastClaim: this.moment(data[i].LastClaim).format(
            "YYYY-MM-DD - hh:mm:ss A"
          )
        });
      }

      this.$store.commit("isLoading", false);
    },
    showLevel2: function() {
      this.$store.commit("isLoading", true);
      this.data.rows.length = 0;
      var data = this.$store.state.level2Referrals;

      for (var i = 0; i < Object.keys(data).length; i++) {
        var totalEarning =
          (this.$store.getters.faucetSettingValue("Level2").Value / 100) *
            data[i].LinkShortnerEarning +
          (this.$store.getters.faucetSettingValue("PTPLevel2").Value / 100) *
            (data[i].PTPUniqueEarning +
              data[i].PTPNonUnique1Earning +
              data[i].PTPNonUnique2Earning +
              data[i].PTPNonUnique3Earning) +
          (this.$store.getters.faucetSettingValue("OfferwallLevel2").Value /
            100) *
            data[i].OfferwallEarning;
        totalEarning = totalEarning.toFixed(8);

        this.data.rows.push({
          Username: data[i].Username,
          TotalCredited: totalEarning,
          LinkShortnerEarning: data[i].LinkShortnerEarning.toFixed(8),
          OfferwallEarning: data[i].OfferwallEarning.toFixed(8),
          PTPUniqueEarning: data[i].PTPUniqueEarning.toFixed(8),
          PTPNonUnique1Earning: data[i].PTPNonUnique1Earning.toFixed(8),
          PTPNonUnique2Earning: data[i].PTPNonUnique2Earning.toFixed(8),
          PTPNonUnique3Earning: data[i].PTPNonUnique3Earning.toFixed(8),
          LastClaim: data[i].LastClaim
        });
      }

      this.$store.commit("isLoading", false);
    },
    showLevel3: function() {
      this.$store.commit("isLoading", true);
      this.data.rows.length = 0;
      var data = this.$store.state.level3Referrals;

      for (var i = 0; i < Object.keys(data).length; i++) {
        var totalEarning =
          (this.$store.getters.faucetSettingValue("Level3").Value / 100) *
            data[i].LinkShortnerEarning +
          (this.$store.getters.faucetSettingValue("PTPLevel3").Value / 100) *
            (data[i].PTPUniqueEarning +
              data[i].PTPNonUnique1Earning +
              data[i].PTPNonUnique2Earning +
              data[i].PTPNonUnique3Earning) +
          (this.$store.getters.faucetSettingValue("OfferwallLevel3").Value /
            100) *
            data[i].OfferwallEarning;
        totalEarning = totalEarning.toFixed(8);

        this.data.rows.push({
          Username: data[i].Username,
          TotalCredited: totalEarning,
          OfferwallEarning: data[i].OfferwallEarning.toFixed(8),
          LinkShortnerEarning: data[i].LinkShortnerEarning.toFixed(8),
          PTPUniqueEarning: data[i].PTPUniqueEarning.toFixed(8),
          PTPNonUnique1Earning: data[i].PTPNonUnique1Earning.toFixed(8),
          PTPNonUnique2Earning: data[i].PTPNonUnique2Earning.toFixed(8),
          PTPNonUnique3Earning: data[i].PTPNonUnique3Earning.toFixed(8),
          LastClaim: data[i].LastClaim
        });
      }

      this.$store.commit("isLoading", false);
    }
  },
  mounted() {
    this.$store.commit("isLoading", true);
    this.data.rows.length = 0;
    this.showLevel1();

    this.$store.commit("isLoading", false);
  }
};
</script>
<style scoped>
thead,
tfoot {
  color: white;
  background-color: #4285f4;
}
</style>
