﻿using System;
using System.Text.RegularExpressions;
using ScrapingFromOurOwn;

namespace ScrapingFromOurOwn
{
	public class FandomQuery
	{
		public int minimum = -1;
		public int maximum = -1;
		public String tag;
		public String custom = "";
		public int results = 0;
		
		public FandomQuery(String tag, int minimum = -1, int maximum = -1, String custom = "")
		{
			this.tag = tag;
			this.minimum = minimum;
			this.maximum = maximum;
			this.custom = custom;
		}
		
		public bool BeginQuery() {
			Regex work_regex = new Regex(@"([\d]+)\sWorks(\sfound|)\sin");
			Regex error_regex = new Regex("div[^\\>]*class=\"[\\w\\s]*errors");
			
			int works = 0;			// number of works: default 0 until a number can be found
			
			String url = "";
			
			if (this.minimum <0 && this.maximum > -1) {
				url = UrlGenerator.SearchUrlMax(this.maximum, this.tag, this.custom);
			} else if (this.maximum < 0 && this.minimum > -1) {
				url = UrlGenerator.SearchUrlMin(this.minimum, this.tag, this.custom);
			} else if (this.maximum > -1 && this.minimum > -1) {
				url = UrlGenerator.SearchUrlMinMax(this.minimum, this.maximum, this.tag, this.custom);
			} else {
				url = UrlGenerator.SearchUrl(this.tag, this.custom);
			}
			String raw = Scraper.Scrape(url);							// scrape search results page 
			
			if(String.IsNullOrEmpty(raw) != true) {
				if(Int32.TryParse(work_regex.Match(raw).Groups[1].ToString().Trim(), out works) == false) {
					return false;
				}
				// if the response is not empty, regex for number and attempt to parse as int
				// if successful: parsed number will be stored in var works, replacing default 0
				// if unsuccessful, default 0 will remain
			} else {
				return false;
			}
			
			// either way, return number of works
			// successful scraping will return number
			// unsuccessful scraping will return 0
			this.results = works;
			
			return true;
		}
	}
}
